using System;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using StringExtensions;

namespace SentryDotNet.AspNetCoreTestApp
{
    public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationOptions>
    {
        public const string SchemeName = "ApiKey";

        public ApiKeyAuthenticationHandler(IOptionsMonitor<ApiKeyAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

#pragma warning disable 1998
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
#pragma warning restore 1998
        {
            var authHeader = Request.Headers["Authorization"].SingleOrDefault(h => h.StartsWith(Scheme.Name, StringComparison.InvariantCultureIgnoreCase));

            if (authHeader == null)
            {
                return AuthenticateResult.NoResult();
            }

            var key = authHeader.TrimStart(Scheme.Name, StringComparison.InvariantCultureIgnoreCase).Trim();

            if (Options.AllowedApiKeys.Contains(key))
            {
                var claimsIdentity = CreateClaimsIdentity();

                Context.User.AddIdentity(claimsIdentity);

                return AuthenticateResult.Success(new AuthenticationTicket(Context.User, new AuthenticationProperties(), Scheme.Name));
            }

            return AuthenticateResult.Fail(Scheme.Name + " invalid api key");
        }

        private ClaimsIdentity CreateClaimsIdentity()
        {
            var claimsIdentity = new ClaimsIdentity(Scheme.Name);

            claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, "the_user"));
            
            return claimsIdentity;
        }
    }
}