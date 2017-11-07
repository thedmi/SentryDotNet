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
                await builder.CaptureAsync();

                throw;
            }
        }

        private SentryEventBuilder CreateEventBuilder(HttpContext context)
        {
            var builder = _client.CreateEventBuilder();

            builder.Sdk = new SentrySdk
            {
                Name = "SentryDotNet.AspNetCore",
                Version = typeof(SentryDotNetMiddleware).Assembly.GetName().Version.ToString(3)
            };
            
            builder.Logger = string.IsNullOrWhiteSpace(builder.Logger) ? "SentryDotNet.AspNetCore" : builder.Logger;
            builder.Culprit = context.Request.Method.ToUpper(CultureInfo.InvariantCulture) + " " +
                              context.Request.Path.ToString();
            
            return builder;
        }
    }
}