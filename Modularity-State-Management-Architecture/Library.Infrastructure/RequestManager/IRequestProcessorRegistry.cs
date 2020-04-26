using System;

namespace StateBuilder.RequestManager
{
    public interface IRequestProcessorRegistry
    {
        void RegisterContextType<TRequest, TResponse, TRequestContext>()
            where TRequestContext : RequestContext<TRequest, TResponse>;

        void RegisterProcessorType<TRequest, TResponse, TRequestProcessor>()
            where TRequestProcessor : IRequestProcessor<TRequest, TResponse>;

        Type ResolveContextType<TRequest, TResponse>();
        Type ResolveProcessorType<TRequest, TResponse>();
    }
}
