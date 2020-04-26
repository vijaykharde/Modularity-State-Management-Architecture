using System;
using System.Collections.Generic;

namespace StateBuilder.RequestManager.Unity
{
    public class RequestProcessorResolverOverridesRegistry : IRequestProcessorResolverOverridesRegistry
    {
        private readonly Dictionary<Type, KeyValuePair<string, object>[]> _resolverOverridesRegistry = new Dictionary<Type, KeyValuePair<string, object>[]>();

        public void RegisterResolverOverrides<TRequestProcessor>(KeyValuePair<string, object>[] resolverOverrides)
        {
            _resolverOverridesRegistry[typeof(TRequestProcessor)] = resolverOverrides;
        }

        public KeyValuePair<string, object>[] GetResolverOverrides(Type requestProcessorType)
        {
            return _resolverOverridesRegistry.TryGetValue(requestProcessorType, out var resolverOverrides) ? resolverOverrides : null;
        }
    }
}
