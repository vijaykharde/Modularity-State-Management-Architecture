using System;

namespace StateBuilder.RequestManager
{
    public interface IRequestObserver
    {
        void OnRequestCompleted<TRequest, TResponse>(RequestContext<TRequest, TResponse> requestContext);
        void OnRequestFailed<TRequest, TResponse>(RequestContext<TRequest, TResponse> requestContext, Exception exception);
        void OnRequestStarted<TRequest, TResponse>(RequestContext<TRequest, TResponse> requestContext);
        void OnRequestSucceeded<TRequest, TResponse>(RequestContext<TRequest, TResponse> requestContext);
    }
}
