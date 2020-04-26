using StateBuilder.Library.Infrastructure;
using StateBuilder.RequestManager;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Unity;
namespace StateBuilder.Library.StateBuilders
{
    public class StateBuilderContext<TStateObject> //: StateHandlerContext
            where TStateObject : class
    {
        private readonly TaskCompletionSource<TStateObject> _completionSource;

        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly IWorkItem _workItem;
        public CancellationToken CancellationToken => _cancellationTokenSource.Token;

        public ReadOnlyDictionary<string, object> Dependencies { get; }
        public ReadOnlyDictionary<string, object> OptionalDependencies { get; }

        public StateBuilderContext(IWorkItem workItem, string stateName, IDictionary<string, object> dependencies, IDictionary<string, object> optionalDependencies, IDictionary<string, object> additionalData)
        {
            _workItem = workItem;
            Dependencies = new ReadOnlyDictionary<string, object>(dependencies);
            OptionalDependencies = new ReadOnlyDictionary<string, object>(optionalDependencies);
            AdditionalData = new ReadOnlyDictionary<string, object>(additionalData);

            _completionSource = new TaskCompletionSource<TStateObject>();

            Task task = _completionSource.Task.ContinueWith(x =>
            {
                workItem.State[stateName] = x.Result;
                //_workItem.State[stateName] = x.Result;
#if DEBUG
                Debug.WriteLine($"************StateBuilderContext**************\n{stateName}\n***************************");
#endif
                _workItem.BroadCastState(_workItem, new BroadCastStateEventArgs(stateName, x.Result));
            }, TaskContinuationOptions.OnlyOnRanToCompletion);
            var taskList = _workItem.Container.Resolve<IList<Task>>();
            taskList.Add(task);
            //task.Start();
        }

        public Task<RequestContext<TRequest, TResponse>> QueueAsync<TRequest, TResponse>(TRequest request, Action<RequestContext<TRequest, TResponse>> onCompleteCallback = null, Action<RequestContext<TRequest, TResponse>, Exception> onExceptionCallback = null)
        {

            CancellationToken.ThrowIfCancellationRequested();
            var requestManager = _workItem.Container.Resolve<IRequestManagerService>();
            Task<RequestContext<TRequest, TResponse>> requestContext = requestManager.QueueAsync<TRequest, TResponse>(request, CancellationToken, onCompleteCallback, onExceptionCallback);

            var taskList = _workItem.Container.Resolve<IList<Task>>();
            taskList.Add(requestContext as Task);
            return requestContext;
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

        public ReadOnlyDictionary<string, object> AdditionalData { get; }

        public void SetResult(TStateObject result)
        {
            _completionSource.TrySetResult(result);
        }

        public void SetException(Exception exception)
        {
            _completionSource.TrySetException(exception);
        }

        public TStateValue GetValue<TStateValue>(string key)
            where TStateValue : class
        {
            if (Dependencies.TryGetValue(key, out var value) || OptionalDependencies.TryGetValue(key, out value) || AdditionalData.TryGetValue(key, out value))
            {
                return (TStateValue)value;
            }
            return null;
        }
    }
}
