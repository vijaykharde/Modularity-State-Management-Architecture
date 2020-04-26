using Library.Infrastructure.Library.StateBuilders;
using StateBuilder.Library.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Unity;
namespace StateBuilder.Library.StateBuilders
{
    public abstract class StateBuilderBase<TStateObject>
            where TStateObject : class
    {
        private readonly IWorkItem _workItem;
        private readonly string _stateName;

        private StateBuilderContext<TStateObject> _stateBuilderContext;

        protected StateBuilderBase(IWorkItem workItem, string stateName)
        {
            _workItem = workItem;
            _stateName = stateName;
            
            var stateRegistry = workItem.RootWorkItem.Container.Resolve<IStateRegistry>();
            stateRegistry.RegisterStateType(workItem.GetType(), _stateName, typeof(TStateObject));
        }

        public void Run()
        {
            if (GetDependencies().Contains(_stateName))
            {
                throw new InvalidOperationException("StateBuilder cannot have itself as a dependency.");
            }
            if (GetOptionalDependencies().Contains(_stateName))
            {
                throw new InvalidOperationException("StateBuilder cannot have itself as a dependency.");
            }

            _workItem.SubscribeStateChangedEvent(_stateName, OnStateChanged);

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

        protected void OnDependencyChanged()
        {
            var dependencies = GetDependencies().ToDictionary(x => x, x => _workItem.State[x]);
            var optionalDependencies = GetOptionalDependencies().ToDictionary(x => x, x => _workItem.State[x]);

            var stateBuilderContext = CreateStateBuilderContext(dependencies, optionalDependencies);
            if (Interlocked.CompareExchange(ref _stateBuilderContext, stateBuilderContext, null) != null)  // try to assign new state handler
            {
                var e = _stateBuilderContext;  // an existing state handler is running, try to cancel and overwrite with new state handler
                while (Interlocked.CompareExchange(ref _stateBuilderContext, stateBuilderContext, e) != e)
                {
                    e?.Cancel();
                    e = _stateBuilderContext;
                }
                e?.Cancel();
            }

            if (stateBuilderContext.Dependencies.Values.All(x => x != null))
            {
                OnDependenciesMet(stateBuilderContext);
            }
            else
            {
                OnDependenciesNotMet(stateBuilderContext);
            }
        }

        protected void OnDependenciesMet(StateBuilderContext stateBuilderContext)
        {
            Task task = Task.Run(async delegate
            {
                try
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(50), stateBuilderContext.CancellationToken);
                    if (!stateBuilderContext.CancellationToken.IsCancellationRequested)
                    {
                        DoWork(stateBuilderContext);
                    }
                }
                catch (TaskCanceledException)
                {
                    stateBuilderContext.Cancel();
                }
                catch (Exception ex)
                {
                    OnException(ex);
                    stateBuilderContext.Cancel();
                }
                finally
                {
                    stateBuilderContext = null;
                }
            }, stateBuilderContext.CancellationToken);
            var taskList = _workItem.Container.Resolve<IList<Task>>();
            taskList.Add(task);
            /*Task.Run(async delegate
            {
                try
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(50), stateBuilderContext.CancellationToken);
                    if (!stateBuilderContext.CancellationToken.IsCancellationRequested)
                    {
                        DoWork(stateBuilderContext);
                    }
                }
                catch (TaskCanceledException)
                {
                    stateBuilderContext.Cancel();
                }
                catch (Exception ex)
                {
                    OnException(ex);
                    stateBuilderContext.Cancel();
                }
                finally
                {
                    stateBuilderContext = null;
                }
            }, stateBuilderContext.CancellationToken);*/
        }

        protected void OnDependenciesNotMet(StateBuilderContext stateBuilderContext)
        {
            stateBuilderContext.SetResult(null);
        }

        protected virtual IEnumerable<string> GetDependencies()
        {
            return Enumerable.Empty<string>();
        }

        protected virtual IEnumerable<string> GetOptionalDependencies()
        {
            return Enumerable.Empty<string>();
        }

        protected virtual IEnumerable<string> GetAdditionalData()
        {
            return Enumerable.Empty<string>();
        }

        protected void DoWork(StateBuilderContext stateBuilderContext)
        {
            GetStateObject(stateBuilderContext);
        }

        protected abstract void GetStateObject(StateBuilderContext context);

        protected StateBuilderContext CreateStateBuilderContext(IDictionary<string, object> dependencies, IDictionary<string, object> optionalDependencies)
        {
            var additionalData = GetAdditionalData().ToDictionary(x => x, x => _workItem.State[x]);
            return new StateBuilderContext(_workItem, _stateName, dependencies, optionalDependencies, additionalData);
        }


        protected class StateBuilderContext : StateBuilderContext<TStateObject>
        {
            public StateBuilderContext(IWorkItem workItem, string stateName, IDictionary<string, object> dependencies, IDictionary<string, object> optionalDependencies, IDictionary<string, object> additionalData)
                : base(workItem, stateName, dependencies, optionalDependencies, additionalData)
            {
            }
        }

        private void OnStateChanged(StateChangedEventArgs eventArgs)
        {
            if (eventArgs.OldValue != null && eventArgs.NewValue == null)
            {
                OnDependencyChanged();
            }
        }

        protected void OnException(Exception ex)
        {
        }
    }
}
