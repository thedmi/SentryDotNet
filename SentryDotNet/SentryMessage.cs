namespace SentryDotNet
{
    /// <summary>
    /// The message interface is a slightly improved version of the message attribute and can be used to split the log message
    /// from the log message parameters.
    /// <para />
    /// https://docs.sentry.io/clientdev/interfaces/message/
    /// </summary>
    public class SentryMessage
    {
        /// <summary>
        /// The formatted message.
        /// </summary>
        public string Formatted => Params == null || Params.Length == 0 ? Message : string.Format(Message, Params);

        /// <summary>
        /// The raw message string (uninterpolated).
        /// Message must be no more than 1000 characters in length.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// An optional list of formatting parameters.
        /// </summary>
        public object[] Params { get; set; }
    }
}