namespace SentryDotNet
{
    public class SentryException : ISentryException
    {
        public string Module { get; set; }

        public SentryStacktrace Stacktrace { get; set; }

        public string Type { get; set; }

        public string Value { get; set; }
    }
}