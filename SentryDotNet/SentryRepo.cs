namespace SentryDotNet
{
    /// <summary>
    /// Describes the local repository configuration for an application.
    /// This is used to map up source code information to stored repository configuration.
    /// <para />
    /// https://docs.sentry.io/clientdev/interfaces/repos/
    /// </summary>
    public class SentryRepo
    {
        /// <summary>
        /// The name of the repository as it is registered in Sentry.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// The optional prefix path to apply to source code when pairing it up with files in the repository.
        /// </summary>
        string Prefix { get; set; }

        /// <summary>
        /// The optional current revision of the local repository.
        /// </summary>
        string Revision { get; set; }
    }
}