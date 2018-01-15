using System.Collections.Generic;

using Microsoft.AspNetCore.Authentication;

namespace SentryDotNet.AspNetCoreTestApp
{
    public class ApiKeyAuthenticationOptions : AuthenticationSchemeOptions  
    {
        public IReadOnlyList<string> AllowedApiKeys { get; set; }
    }
}