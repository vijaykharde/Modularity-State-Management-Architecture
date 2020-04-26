using System.Linq;
using Unity;
using Unity.Resolution;

namespace StateBuilder.RequestManager.Unity
{
    public class RequestProcessorResolver : IRequestProcessorResolver
    {
        private readonly IUnityContainer _container;

        public RequestProcessorResolver(IUnityContainer container)
        {
            _container = container;
        }

        public RequestContext<TRequest, TResponse> ResolveContext<TRequest, TResponse>()
        {
            var registry = _container.Resolve<IRequestProcessorRegistry>();
            var contextType = registry.ResolveContextType<TRequest, TResponse>();
            return (RequestContext<TRequest, TResponse>)_container.Resolve(contextType);
        }

        public IRequestProcessor<TRequest, TResponse> ResolveProcessor<TRequest, TResponse>()
        {
            var requestProcessorRegistry = _container.Resolve<IRequestProcessorRegistry>();
            var requestProcessorType = requestProcessorRegistry.ResolveProcessorType<TRequest, TResponse>();
            var resolverOverridesRegistry = _container.Resolve<IRequestProcessorResolverOverridesRegistry>();
            var resolverOverrides = resolverOverridesRegistry.GetResolverOverrides(requestProcessorType);
            if (resolverOverrides != null)
            {
                return (IRequestProcessor<TRequest, TResponse>)_container.Resolve(requestProcessorType, resolverOverrides.Select(x => new ParameterOverride(x.Key, x.Value)).Cast<ResolverOverride>().ToArray());
            }
            return (IRequestProcessor<TRequest, TResponse>)_container.Resolve(requestProcessorType);
        }
    }
}
