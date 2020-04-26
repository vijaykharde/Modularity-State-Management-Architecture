namespace StateBuilder.RequestManager
{
    public interface IRequestProcessorResolver
    {
        RequestContext<TRequest, TResponse> ResolveContext<TRequest, TResponse>();
        IRequestProcessor<TRequest, TResponse> ResolveProcessor<TRequest, TResponse>();
    }
}
