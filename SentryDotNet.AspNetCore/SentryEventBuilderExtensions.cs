using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;

using Microsoft.AspNetCore.Http;

namespace SentryDotNet.AspNetCore
{
    public static class SentryEventBuilderExtensions
    {
        public static SentryEventBuilder AddRequestInformation(this SentryEventBuilder builder, HttpContext context)
        {
            var request = context.Request;

            builder.Request = new HttpSentryContext
            {
                Url = $"{request.Scheme}://{request.Host}{request.Path}",
                Method = request.Method.ToUpper(CultureInfo.InvariantCulture),
                QueryString = request.QueryString.ToString(),
                Headers = request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()),
                Env = new Dictionary<string, string> { { "REMOTE_ADDR", context.Connection.RemoteIpAddress.ToString() } }
            };

            return builder;
        }

        public static SentryEventBuilder AddUserInformation(this SentryEventBuilder builder, HttpContext context)
        {
            if (context.User != null)
            {
                builder.User = new UserSentryContext
                {
                    Id = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                    Username = context.User.FindFirst(ClaimTypes.Name)?.Value,
                    Email = context.User.FindFirst(ClaimTypes.Email)?.Value,
                    IpAddress = context.Connection.RemoteIpAddress.ToString()
                };
            }

            return builder;
        }
    }
}