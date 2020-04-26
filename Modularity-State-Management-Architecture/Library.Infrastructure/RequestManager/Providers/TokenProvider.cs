using StateBuilder.Library.Security.Identities;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;

namespace StateBuilder.RequestManager.Providers
{
    public class TokenProvider : ITokenProvider
    {
        public IToken GetAuthenticationToken()
        {
            var claimsPrincipal = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var asacIdentity = claimsPrincipal.Identities.OfType<AsacIdentity>().SingleOrDefault();
            if (asacIdentity == null)
            {
                throw new Exception("AsacIdentity not found on principal object!");
            }
            return new Token
            {
                Expiration = asacIdentity.TokenExpiration,
                Value = asacIdentity.Token
            };
        }

        public IToken GetIafToken()
        {
            var claimsPrincipal = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var iafIdentity = claimsPrincipal.Identities.OfType<IafIdentity>().SingleOrDefault();
            if (iafIdentity == null)
            {
                throw new Exception("IafIdentity not found on principal object!");
            }
            return new Token
            {
                Expiration = iafIdentity.TokenExpiration,
                Value = iafIdentity.Token
            };
        }

        private class Token : IToken
        {
            public DateTime Expiration { get; set; }
            public string Value { get; set; }
        }
    }
}
