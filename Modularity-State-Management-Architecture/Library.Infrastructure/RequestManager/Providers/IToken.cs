using System;

namespace StateBuilder.RequestManager.Providers
{
    public interface IToken
    {
        DateTime Expiration { get; }
        string Value { get; }
    }
}
