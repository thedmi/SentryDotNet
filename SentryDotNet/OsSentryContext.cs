namespace SentryDotNet
{
    /// <summary>
    /// Defines the operating system that caused the event. In web contexts, this is the operating system of the browser 
    /// (normally pulled from the User-Agent string).
    /// <para />
    /// https://docs.sentry.io/clientdev/interfaces/contexts/
    /// </summary>
    public class OsSentryContext : ISentryContext
    {
        public string Type => ContextTypes.Os;

        /// <summary>
        /// The name of the operating system.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The version of the operating system.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// The internal build revision of the operating system.
        /// </summary>
        public string Build { get; set; }

        /// <summary>
        /// If known this can be an independent kernel version string. Typically this is something like the entire output of the
        /// uname tool.
        /// </summary>
        public string KernelVersion { get; set; }

        /// <summary>
        /// An optional bool that defines if the OS has been jailbroken or rooted.
        /// </summary>
        public bool Rooted { get; set; }
    }
}