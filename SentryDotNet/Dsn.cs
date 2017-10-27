using System;

namespace SentryDotNet
{
    /// <summary>
    /// A Data Source Name, or dsn, for a specific project.
    /// </summary>
    public class Dsn
    {
        private readonly string _dsn;
        
        public Dsn(string dsn)
        {
            _dsn = dsn;

            try
            {
                var uri = new Uri(dsn);

                PrivateKey = uri.UserInfo.Split(':')[1];
                PublicKey = uri.UserInfo.Split(':')[0];

                Port = uri.Port;

                ProjectId = uri.AbsoluteUri.Substring(uri.AbsoluteUri.LastIndexOf("/", StringComparison.Ordinal) + 1);

                Path = uri.AbsolutePath.Substring(0, uri.AbsolutePath.LastIndexOf("/", StringComparison.Ordinal));

                ReportApiUri = new Uri(string.Format("{0}://{1}:{2}{3}/api/{4}/store/",
                    uri.Scheme,
                    uri.DnsSafeHost,
                    Port,
                    Path,
                    ProjectId));
            }
            catch (Exception exception)
            {
                throw new ArgumentException("Invalid DSN", "dsn", exception);
            }
        }
        
        /// <summary>
        /// Sentry path.
        /// </summary>
        public string Path { get; }

        /// <summary>
        /// The sentry server port.
        /// </summary>
        public int Port { get; }

        /// <summary>
        /// Project private key.
        /// </summary>
        public string PrivateKey { get; }

        /// <summary>
        /// Project identification.
        /// </summary>
        public string ProjectId { get; }

        /// <summary>
        /// Project public key.
        /// </summary>
        public string PublicKey { get; }

        /// <summary>
        /// Sentry Uri for sending reports.
        /// </summary>
        public Uri ReportApiUri { get; }

        public override string ToString()
        {
            return _dsn;
        }
    }
}