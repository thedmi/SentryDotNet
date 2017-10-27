using System;

namespace SentryDotNet
{
    public class SentryBreadcrumb : ISentryBreadcrumb
    {
        public SentryBreadcrumb(string category)
        {
            Category = category;
        }

        public string Category { get; set; }

        public object Data { get; set; }

        public SeverityLevel Level { get; set; } = SeverityLevel.Info;

        public string Message { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public BreadcrumbType Type { get; set; } = BreadcrumbType.Default;
    }
}