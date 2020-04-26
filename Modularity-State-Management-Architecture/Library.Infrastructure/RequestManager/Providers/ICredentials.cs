using System.Security;

namespace StateBuilder.RequestManager.Providers
{
    public interface ICredentials
    {
        string Username { get; }
        SecureString Password { get; }
    }
}
