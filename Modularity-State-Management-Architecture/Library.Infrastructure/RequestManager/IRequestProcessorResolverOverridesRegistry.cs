using System;
using System.Collections.Generic;

namespace StateBuilder.RequestManager
{
    public interface IRequestProcessorResolverOverridesRegistry
    {
        void RegisterResolverOverrides<TRequestProcessor>(KeyValuePair<string, object>[] resolverOverrides);
        KeyValuePair<string, object>[] GetResolverOverrides(Type requestProcessorType);
    }
}
