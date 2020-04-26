using StateBuilder.Library.Infrastructure;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StateBuilder.RequestManager
{
    public class RequestManagerService : IRequestManagerService
    {
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly IRequestObserver _requestObserver;
        private readonly IRequestProcessorResolver _requestProcessorResolver;

        public RequestManagerService(IRequestProcessorResolver requestProcessorResolver)
            : this(null, requestProcessorResolver)
        {
        }

        public RequestManagerService(IRequestObserver requestObserver, IRequestProcessorResolver requestProcessorResolver)
        {
            _requestObserver = requestObserver;
            _requestProcessorResolver = requestProcessorResolver;
        }

        public Task<RequestContext<TRequest, TResponse>> QueueAsync<TRequest, TResponse>(TRequest request, CancellationToken? cancellationToken = null, Action<RequestContext<TRequest, TResponse>> onCompleteCallback = null, Action<RequestContext<TRequest, TResponse>, Exception> onExceptionCallback = null)
        {
            var requestContext = _requestProcessorResolver.ResolveContext<TRequest, TResponse>();
            if (requestContext == null)
            {
                throw new Exception(); // TODO: Provide a more meaningful message
            }

            var requestProcessor = _requestProcessorResolver.ResolveProcessor<TRequest, TResponse>();
            if (requestProcessor == null)
            {
                throw new Exception(); // TODO: Provide a more meaningful message
            }

            requestProcessor.AssertRequest(request);

            var linkedTokenSource = cancellationToken != null ? CancellationTokenSource.CreateLinkedTokenSource(_cancellationTokenSource.Token, cancellationToken.Value) : _cancellationTokenSource;

            return Task.Run(async delegate
            {
                try
                {
                    _requestObserver?.OnRequestStarted(requestContext);

                    requestContext.Request = request;
                    requestContext.OnCompleteCallback = onCompleteCallback;
                    requestContext.OnExceptionCallback = onExceptionCallback;

                    var startTime = DateTime.UtcNow;
                    try
                    {
                        var subTask = requestProcessor.ProcessAsync(requestContext, linkedTokenSource.Token);
                        await subTask;
                    }
                    finally
                    {
                        var totalTimeMetric = new RequestMetricData
                        {
                            SampleTime = startTime,
                            MetricName = "TotalTime",
                            MetricValue = (DateTime.UtcNow - startTime).TotalMilliseconds
                        };
                        requestContext.Metrics = requestContext.Metrics?.Union(
                            new[] { totalTimeMetric }
                        ).ToArray() ?? new[] { totalTimeMetric };
                        _requestObserver?.OnRequestCompleted(requestContext);
                    }
                    requestContext.OnCompleteCallback?.Invoke(requestContext);
                    _requestObserver?.OnRequestSucceeded(requestContext);
                    return requestContext;
                }
                catch (Exception ex)
                {
                    requestContext.OnExceptionCallback?.Invoke(requestContext, ex);
                    _requestObserver?.OnRequestFailed(requestContext, ex);
                    throw;
                }
            },
                linkedTokenSource.Token
            );
            //return task;
        }

        public async Task<TResponse> ProcessAsync<TRequest, TResponse>(TRequest request, CancellationToken? cancellationToken = null)
        {
            return (await QueueAsync<TRequest, TResponse>(request, cancellationToken)).Response;
        }

        public void CancelAllRequests()
        {
            _cancellationTokenSource.Cancel();
        }
    }
}
