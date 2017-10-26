namespace SentryDotNet
{
    /// <summary>
    /// The level of severity.
    /// </summary>
    public enum SeverityLevel
    {
        /// <summary>
        /// This level is reserved for items that are more critical than <see cref="Error" /> and are of paramount importance.
        /// Messages with this level will display as dark red in Sentry.
        /// </summary>
        Fatal,

        /// <summary>
        /// This level is reserved for items that shouldn't be ignored. Messages with this level will display as bright red in
        /// Sentry.
        /// </summary>
        Error,

        /// <summary>
        /// This level is reserved for items that are less severe than <see cref="Error" />. Messages with this level will
        /// display as orange in Sentry.
        /// </summary>
        Warning,

        /// <summary>
        /// This level is reserved for items that are less severe than <see cref="Warning" /> and are typically used for
        /// informational purposes. Messages with this level will display as blue in Sentry.
        /// </summary>
        Info,

        /// <summary>
        /// This level is reserved for items that are even less severe than <see cref="Info" /> and are typically used for
        /// development efforts. Messages with this level will display as grey in Sentry.
        /// </summary>
        Debug
    }
}