using System;
using System.Collections.Generic;

namespace StateBuilder.RequestManager
{
    public class RequestProcessorRegistry : IRequestProcessorRegistry
    {
        private readonly Dictionary<(Type, Type), Type> _requestContextRegistry = new Dictionary<(Type, Type), Type>();
        private readonly Dictionary<(Type, Type), Type> _requestProcessorRegistry = new Dictionary<(Type, Type), Type>();
        private readonly IRequestProcessorRegistry _parent;

        public RequestProcessorRegistry()
            : this(null)
        {
        }

        public RequestProcessorRegistry(IRequestProcessorRegistry parent)
        {
            _parent = parent;
        }

        public void RegisterContextType<TRequest, TResponse, TRequestContext>()
            where TRequestContext : RequestContext<TRequest, TResponse>
        {
            _requestContextRegistry[(typeof(TRequest), typeof(TResponse))] = typeof(TRequestContext);
        }

        public void RegisterProcessorType<TRequest, TResponse, TRequestProcessor>()
            where TRequestProcessor : IRequestProcessor<TRequest, TResponse>
        {
            _requestProcessorRegistry[(typeof(TRequest), typeof(TResponse))] = typeof(TRequestProcessor);
        }

        public Type ResolveContextType<TRequest, TResponse>()
        {
            if (_requestContextRegistry.TryGetValue((typeof(TRequest), typeof(TResponse)), out var requestContextType))
            {
                return requestContextType;
            }
            return _parent?.ResolveContextType<TRequest, TResponse>() ?? typeof(RequestContext<TRequest, TResponse>);
        }

        public Type ResolveProcessorType<TRequest, TResponse>()
        {
            if (_requestProcessorRegistry.TryGetValue((typeof(TRequest), typeof(TResponse)), out var requestProcessorType))
            {
                return requestProcessorType;
            }
            return _parent?.ResolveProcessorType<TRequest, TResponse>();
        }
    }
}
