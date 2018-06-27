using System;
using System.Globalization;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

namespace SentryDotNet.AspNetCore
{
    public class SentryDotNetMiddleware
    {
        public const string EventBuilderKey = "SentryEventBuilder";
        
        private readonly RequestDelegate _next;

        private readonly ISentryClient _client;

        private readonly SentryDotNetOptions _options;

        public SentryDotNetMiddleware(RequestDelegate next, ISentryClient client, SentryDotNetOptions options)
        {
            _next = next;
            _client = client;
            _options = options;

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
                if (e is SentryClientException)
                {
                    Console.Error.WriteLine("Exception during communication with Sentry:");
                    Console.Error.WriteLine(e.ToString());
                    
                    return;
                }

                // The user information needs to be added here, because during builder initialization
                // the secondary authentication handlers have not yet run. This may be solved with 
                // ASP.NET Core 2.1, see https://github.com/aspnet/Security/issues/1469 .
                builder.AddUserInformation(context);
                
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

            builder.AddRequestInformation(context, _options);
            
            return builder;
        }
    }
}