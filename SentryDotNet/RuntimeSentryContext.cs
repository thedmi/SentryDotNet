namespace SentryDotNet
{
    /// <summary>
    /// This describes a runtime in more detail. Typically this context is used multiple times if multiple runtimes are 
    /// involved (for instance if you have a JavaScript application running on top of JVM)
    /// <para />
    /// https://docs.sentry.io/clientdev/interfaces/contexts/
    /// </summary>
    public class RuntimeSentryContext : ISentryContext
    {
        public string Type => ContextTypes.Runtime;

        /// <summary>
        /// The name of the runtime.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The version identifier of the runtime.
        /// </summary>
        public string Version { get; set; }
    }
}