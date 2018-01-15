using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

namespace SentryDotNet.AspNetCore
{
    public class SentryDotNetMiddleware
    {
        public const string EventBuilderKey = "SentryEventBuilder";
        
        private readonly RequestDelegate _next;

        private readonly ISentryClient _client;

        public SentryDotNetMiddleware(RequestDelegate next, ISentryClient client)
        {
            _next = next;
            _client = client;

            if (client?.Dsn == null)
            {
                Console.WriteLine("SentryClient has been disabled, exceptions won't be captured");
            }
        }

        public async Task Invoke(HttpContext context)
        {
            var builder = CreateEventBuilder(context);

            context.Items.Add(EventBuilderKey, builder);
            
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception e) when (_client != null)
            {
                builder.SetException(e);
                await SendToSentryAsync(builder, e);
                
                throw;
            }
        }

        private static async Task SendToSentryAsync(SentryEventBuilder builder, Exception e)
        {
            try
            {
                await builder.CaptureAsync();
            }
            catch (SentryClientException sentryClientException)
            {
                Console.Error.WriteLine("Exception during communication with Sentry:");
                Console.Error.WriteLine(sentryClientException.ToString());
                Console.Error.WriteLine("The following exception was thus NOT reported to Sentry:");
                Console.Error.WriteLine(e.ToString());
                Console.Error.WriteLine();
            }
        }

        private SentryEventBuilder CreateEventBuilder(HttpContext context)
        {
            var request = context.Request;

            var builder = _client.CreateEventBuilder();

            builder.Sdk = new SentrySdk
            {
                Name = "SentryDotNet.AspNetCore",
                Version = typeof(SentryDotNetMiddleware).Assembly.GetName().Version.ToString(3)
            };
            
            builder.Logger = string.IsNullOrWhiteSpace(builder.Logger) ? "SentryDotNet.AspNetCore" : builder.Logger;
            builder.Culprit = request.Method.ToUpper(CultureInfo.InvariantCulture) + " " + request.Path.ToString();

            var clientIp = context.Connection.RemoteIpAddress.ToString();
            
            builder.Request = new HttpSentryContext
            {
                Url = $"{request.Scheme}://{request.Host}{request.Path}",
                Method = request.Method.ToUpper(CultureInfo.InvariantCulture),
                QueryString = request.QueryString.ToString(),
                Headers = request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()),
                Env = new Dictionary<string, string> { { "REMOTE_ADDR", clientIp } }
            };

            if (context.User != null)
            {
                builder.User = new UserSentryContext
                {
                    Id = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                    Username = context.User.FindFirst(ClaimTypes.Name)?.Value,
                    Email = context.User.FindFirst(ClaimTypes.Email)?.Value,
                    IpAddress = clientIp
                };
            }
            
            return builder;
        }
    }
}