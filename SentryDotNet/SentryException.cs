namespace SentryDotNet
{
    /// <summary>
    /// An exception consists of a list of values. In most cases, this list contains a single exception, with an optional
    /// stacktrace interface.
    /// <para />
    /// https://docs.sentry.io/clientdev/interfaces/exception/
    /// </summary>
    public class SentryException
    {
        /// <summary>
        /// The optional module, or package which the exception type lives in.
        /// </summary>
        public string Module { get; set; }

        /// <summary>
        /// The exceptions stacktrace.
        /// </summary>
        public SentryStacktrace Stacktrace { get; set; }

        /// <summary>
        /// The type of exception, e.g. ValueError.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// The value of the exception (a string).
        /// </summary>
        public string Value { get; set; }
    }
}