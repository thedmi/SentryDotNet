using System;
using System.Threading.Tasks;
using Xunit;

namespace SentryDotNet.Test
{
    public class ManualTests
    {
        private readonly string _dsn ="";
        
        [Fact]
        public async Task TestReportException()
        {
            var client = new SentryClient(_dsn);
            
            try
            {
                SomeMethod(42);
            }
            catch (InvalidOperationException e)
            {
                await client.CaptureAsync(e);
            }
        }
        
        [Fact]
        public async Task TestReportExceptionWithBreadcrumbsAndAdditionalInfo()
        {
            var client = new SentryClient(_dsn);

            var sentryEventBuilder = client.CreateEventBuilder();

            sentryEventBuilder.Level = SeverityLevel.Warning;
            
            sentryEventBuilder.Breadcrumbs.Add(new SentryBreadcrumb("bread.crumb.1"));
            sentryEventBuilder.Breadcrumbs.Add(new SentryBreadcrumb("bread.crumb.2") { Level = SeverityLevel.Warning });
            
            try
            {
                SomeMethod(42);
            }
            catch (InvalidOperationException e)
            {
                sentryEventBuilder.SetException(e);
                await sentryEventBuilder.CaptureAsync();
            }
        }

        // ReSharper disable once UnusedParameter.Local
        private void SomeMethod(int theParam)
        {
            FailingMethod();
        }

        private void FailingMethod()
        {
            throw new InvalidOperationException("It failed deliberately :-)");
        }
    }
}