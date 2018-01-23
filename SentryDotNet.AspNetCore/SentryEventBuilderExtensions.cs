using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;

using Microsoft.AspNetCore.Http;

namespace SentryDotNet.AspNetCore
{
    public static class SentryEventBuilderExtensions
    {
        public static SentryEventBuilder AddRequestInformation(
            this SentryEventBuilder builder,
            HttpContext context,
            SentryDotNetOptions options)
        {
            var request = context.Request;

            builder.Request = new HttpSentryContext
            {
                Url = $"{request.Scheme}://{request.Host}{request.Path}",
                Method = request.Method.ToUpper(CultureInfo.InvariantCulture),
                QueryString = request.QueryString.ToString(),
                Headers = request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()),
                Env = new Dictionary<string, string> { { "REMOTE_ADDR", context.Connection.RemoteIpAddress.ToString() } },
                Data = options.CaptureRequestBody ? ReadBody(request, options) : null
            };

            return builder;
        }

        private static object ReadBody(HttpRequest request, SentryDotNetOptions options)
        {
            return options.CaptureRequestBodyConverter(request);
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