namespace SentryDotNet
{
    /// <summary>
    /// The message interface is a slightly improved version of the message attribute and can be used to split the log message
    /// from the log message parameters.
    /// <para />
    /// https://docs.sentry.io/clientdev/interfaces/message/
    /// </summary>
    public interface ISentryMessage
    {
        /// <summary>
        /// The formatted message.
        /// </summary>
        string Formatted { get; }

        /// <summary>
        /// The raw message string (uninterpolated).
        /// Message must be no more than 1000 characters in length.
        /// </summary>
        string Message { get; }

        /// <summary>
        /// An optional list of formatting parameters.
        /// </summary>
        object[] Params { get; }
    }
}