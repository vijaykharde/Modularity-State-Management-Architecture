using StateBuilder.RequestManager;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Unity;
namespace StateBuilder.Library.Infrastructure.StateHandlers
{
    public class StateHandlerContext
    {
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly IWorkItem _workItem;

        public StateHandlerContext(IWorkItem workItem, IDictionary<string, object> dependencies, IDictionary<string, object> optionalDependencies)
        {
            _workItem = workItem;

            Dependencies = new ReadOnlyDictionary<string, object>(dependencies);
            OptionalDependencies = new ReadOnlyDictionary<string, object>(optionalDependencies);
        }

        public CancellationToken CancellationToken => _cancellationTokenSource.Token;

        public ReadOnlyDictionary<string, object> Dependencies { get; }
        public ReadOnlyDictionary<string, object> OptionalDependencies { get; }

        public Task<RequestContext<TRequest, TResponse>> QueueAsync<TRequest, TResponse>(TRequest request, Action<RequestContext<TRequest, TResponse>> onCompleteCallback = null, Action<RequestContext<TRequest, TResponse>, Exception> onExceptionCallback = null)
        {
            CancellationToken.ThrowIfCancellationRequested();
            var requestManager = _workItem.Container.Resolve<IRequestManagerService>();
            return requestManager.QueueAsync<TRequest, TResponse>(request, CancellationToken, onCompleteCallback, onExceptionCallback);
        }

        public Task<TResponse> ProcessAsync<TRequest, TResponse>(TRequest request)
        {
            var requestManager = _workItem.Container.Resolve<IRequestManagerService>();
            return requestManager.ProcessAsync<TRequest, TResponse>(request, CancellationToken);
        }

        public void Cancel()
        {
            _cancellationTokenSource.Cancel();
        }

        public virtual TStateValue GetValue<TStateValue>(string key)
            where TStateValue : class
        {
            if (Dependencies.TryGetValue(key, out var value) || OptionalDependencies.TryGetValue(key, out value))
            {
                return (TStateValue)value;
            }
            return null;
        }
    }
}
