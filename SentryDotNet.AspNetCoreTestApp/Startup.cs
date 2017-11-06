using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

using SentryDotNet.AspNetCore;

namespace SentryDotNet.AspNetCoreTestApp
{
    public class Startup
    {
        private readonly IHostingEnvironment _env;

        public Startup(IHostingEnvironment env)
        {
            _env = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Add a DSN for test purposes here:
            var dsn = "";
            
            services.AddSentryDotNet(
                new SentryClient(
                    dsn,
                    new SentryEventDefaults(
                        environment: _env.ApplicationName,
                        release: typeof(ISentryClient).Assembly.GetName().Version.ToString(3),
                        logger: "SentryDotNet")));
        }

        public void Configure(IApplicationBuilder app)
        {
            // Make sure middleware that catches exceptions without rethrowing them is added *before* SentryDotNet
            app.UseDeveloperExceptionPage();
            
            app.UseSentryDotNet();
            
            app.Run(async context => { await DoSomethingAsync(context); });
        }

        private static async Task DoSomethingAsync(HttpContext context)
        {
            var eventBuilder = (SentryEventBuilder)context.Items[SentryDotNetMiddleware.EventBuilderKey];
            
            eventBuilder.Breadcrumbs.Add(new SentryBreadcrumb("some.breadcrumb") { Message = "I am a breadcrumb" });
            
            if (context.Request.Path.HasValue && context.Request.Path.Value.Contains("error"))
            {
                throw new InvalidOperationException("Boom");
            }

            await context.Response.WriteAsync("All good.");
        }
    }
}