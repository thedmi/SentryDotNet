namespace SentryDotNet
{
    /// <summary>
    /// An exception consists of a list of values. In most cases, this list contains a single exception, with an optional
    /// stacktrace interface.
    /// <para />
    /// https://docs.sentry.io/clientdev/interfaces/exception/
    /// </summary>
    public interface ISentryException
    {
        /// <summary>
        /// The optional module, or package which the exception type lives in.
        /// </summary>
        string Module { get; }

        /// <summary>
        /// The exceptions stacktrace.
        /// </summary>
        SentryStacktrace Stacktrace { get; }

        /// <summary>
        /// The type of exception, e.g. ValueError.
        /// </summary>
        string Type { get; }

        /// <summary>
        /// The value of the exception (a string).
        /// </summary>
        string Value { get; }
    }
}