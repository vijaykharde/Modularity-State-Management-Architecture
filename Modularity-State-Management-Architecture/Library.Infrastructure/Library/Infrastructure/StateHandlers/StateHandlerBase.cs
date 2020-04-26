using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StateBuilder.Library.Infrastructure.StateHandlers
{
    public abstract class StateHandlerBase
    {
        private readonly IWorkItem _workItem;
        private StateHandlerContext _stateHandlerContext;

        protected StateHandlerBase(IWorkItem workItem)
        {
            _workItem = workItem;
        }

        public void Run()
        {
            OnRunStarted();
        }

        protected virtual void OnRunStarted()
        {
            var dependencies = GetDependencies().ToArray();
            foreach (var dependencyName in dependencies)
            {
                _workItem.SubscribeStateChangedEvent(dependencyName, OnDependencyChanged);
            }
            if (!dependencies.Any())
            {
                OnDependencyChanged();
            }

            var optional = GetOptionalDependencies();
            foreach (var dependencyName in optional)
            {
                _workItem.SubscribeStateChangedEvent(dependencyName, OnDependencyChanged);
            }
        }

        protected virtual IEnumerable<string> GetDependencies()
        {
            return Enumerable.Empty<string>();
        }

        protected virtual IEnumerable<string> GetOptionalDependencies()
        {
            return Enumerable.Empty<string>();
        }

        protected void OnDependencyChanged()
        {
            var dependencies = GetDependencies().ToDictionary(x => x, x => _workItem.State[x]);
            var optionalDependencies = GetOptionalDependencies().ToDictionary(x => x, x => _workItem.State[x]);

            var stateHandlerContext = CreateStateHandlerContext(dependencies, optionalDependencies);
            if (Interlocked.CompareExchange(ref _stateHandlerContext, stateHandlerContext, null) != null)  // try to assign new state handler
            {
                var e = _stateHandlerContext;  // an existing state handler is running, try to cancel and overwrite with new state handler
                while (Interlocked.CompareExchange(ref _stateHandlerContext, stateHandlerContext, e) != e)
                {
                    e?.Cancel();
                    e = _stateHandlerContext;
                }
                e?.Cancel();
            }

            if (stateHandlerContext.Dependencies.Values.All(x => x != null))
            {
                OnDependenciesMet(stateHandlerContext);
            }
            else
            {
                OnDependenciesNotMet(stateHandlerContext);
            }
        }

        protected virtual void OnDependenciesMet(StateHandlerContext stateHandlerContext)
        {
            Task.Run(async delegate
            {
                try
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(50), stateHandlerContext.CancellationToken);
                    if (!stateHandlerContext.CancellationToken.IsCancellationRequested)
                    {
                        DoWork(stateHandlerContext);
                    }
                }
                catch (TaskCanceledException)
                {
                    stateHandlerContext.Cancel();
                }
                catch (Exception ex)
                {
                    OnException(ex);
                    stateHandlerContext.Cancel();
                }
                finally
                {
                    stateHandlerContext = null;
                }
            }, stateHandlerContext.CancellationToken);
        }

        protected virtual void OnDependenciesNotMet(StateHandlerContext stateHandlerContext)
        {
        }

        protected abstract void DoWork(StateHandlerContext stateHandlerContext);

        protected virtual StateHandlerContext CreateStateHandlerContext(IDictionary<string, object> dependencies, IDictionary<string, object> optionalDependencies)
        {
            return new StateHandlerContext(_workItem, dependencies, optionalDependencies);
        }

        protected virtual void OnException(Exception ex)
        {
        }
    }
}
