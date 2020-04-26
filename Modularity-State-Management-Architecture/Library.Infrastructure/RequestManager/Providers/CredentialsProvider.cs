using StateBuilder.Library.Security.Extensions;
using System.Security;

namespace StateBuilder.RequestManager.Providers
{
    public class CredentialsProvider : ICredentialsProvider
    {
        private readonly ICredentials _credentials;

        public CredentialsProvider(string username, string password)
        {
            _credentials = new Credentials(username, password);
        }

        public ICredentials GetCredentials()
        {
            return _credentials;
        }

        private class Credentials : ICredentials
        {
            public Credentials(string username, string password)
            {
                Username = username;
                Password = password.ToSecureString();
            }

            public string Username { get; }
            public SecureString Password { get; }
        }
    }
}
