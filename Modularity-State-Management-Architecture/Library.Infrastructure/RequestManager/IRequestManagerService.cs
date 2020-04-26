using System;
using System.Threading;
using System.Threading.Tasks;

namespace StateBuilder.RequestManager
{
    public interface IRequestManagerService
    {
        Task<RequestContext<TRequest, TResponse>> QueueAsync<TRequest, TResponse>(TRequest request, CancellationToken? cancellationToken = null, Action<RequestContext<TRequest, TResponse>> onCompleteCallback = null, Action<RequestContext<TRequest, TResponse>, Exception> onExceptionCallback = null);
        Task<TResponse> ProcessAsync<TRequest, TResponse>(TRequest request, CancellationToken? cancellationToken = null);
        void CancelAllRequests();
    }
}
