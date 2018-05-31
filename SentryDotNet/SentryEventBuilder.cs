using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SentryDotNet
{
    /// <summary>
    /// This builder simplifies data aggregation that leads to a single event. If you want to add breadcrumbs
    /// and then an exception for example, this class is the way to go. Note that one builder must be used for a single
    /// event.
    /// </summary>
    public class SentryEventBuilder
    {
        private readonly ISentryClient _client;

        public SentryEventBuilder(ISentryClient client)
        {
            _client = client;
        }

        /// <summary>
        /// Hexadecimal string representing a uuid4 value.
        /// </summary>
        public Guid? EventId { get; set; }

        /// <summary>
        /// Indicates when the logging record was created.
        /// </summary>
        public DateTime? Timestamp { get; set; }

        /// <summary>
        /// The name of the logger from which the event was created.
        /// </summary>
        public string Logger { get; set; }

        /// <summary>
        /// The platform the client was using when the event was created.
        /// This will be used by Sentry to alter various UI components.
        /// <para />
        /// Acceptable Values: as3, c, cfml, cocoa, csharp, go, java, javascript, node, objc, other, perl, php, python, and ruby
        /// </summary>
        public string Platform { get; set; } = "csharp";

        /// <summary>
        /// Information about the <see cref="SentrySdk" /> sending the event.
        /// </summary>
        public SentrySdk Sdk { get; set; } = SentrySdk.SentryDotNet;

        /// <summary>
        /// The record <see cref="SeverityLevel" />.
        /// </summary>
        public SeverityLevel? Level { get; set; }

        /// <summary>
        /// The name of the transaction (or culprit) which caused this event to be created.
        /// </summary>
        public string Culprit { get; set; }

        /// <summary>
        /// The name of the host client from which the event was created e.g. foo.example.com.
        /// </summary>
        public string ServerName { get; set; }

        /// <summary>
        /// The version of the application.
        /// </summary>
        public string Release { get; set; }

        /// <summary>
        /// A list of tags, or additional information, for this event.
        /// </summary>
        public Dictionary<string, string> Tags { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// The operating environment that the event was created e.g. production, staging.
        /// </summary>
        public string Environment { get; set; }

        /// <summary>
        /// A list of relevant modules and their versions.
        /// </summary>
        public Dictionary<string, string> Modules { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// An arbitrary mapping of additional metadata to store with the event.
        /// </summary>
        public object Extra { get; set; }

        /// <summary>
        /// An array of strings used to dictate the deduplication of this event.
        /// </summary>
        public IReadOnlyList<string> Fingerprint { get; set; } = new List<string>();

        /// <summary>
        /// A list of <see cref="ISentryException" /> related to this event.
        /// </summary>
        public List<ISentryException> Exception { get; set; } = new List<ISentryException>();

        /// <summary>
        /// A user friendly event that conveys the meaning of this event.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// A trail of breadcrumbs, if any, that led up to the event creation.
        /// </summary>
        public List<ISentryBreadcrumb> Breadcrumbs { get; set; } = new List<ISentryBreadcrumb>();
        
        /// <summary>
        /// The HTTP request context, if any.
        /// </summary>
        public IHttpSentryContext Request { get; set; }

        /// <summary>
        /// Information about the user that triggered the event.
        /// </summary>
        public IUserSentryContext User { get; set; }

        /// <summary>
        /// A dictionary of <see cref="ISentryContext" /> for this event.
        /// </summary>
        public Dictionary<string, ISentryContext> Contexts { get; set; } = new Dictionary<string, ISentryContext>();

        // TODO Add User and Threads "interfaces"

        public void SetMessage(FormattableString message)
        {
            if (Level == null)
            {
                Level = SeverityLevel.Info;
            }
            
            Message = message.ToString();
            Fingerprint = new [] { message.Format, Level.ToString() };
        }
        
        public void SetMessage(object message)
        {
            if (Level == null)
            {
                Level = SeverityLevel.Info;
            }
            
            Message = message.ToString();
        }
        
        public void SetException(Exception ex)
        {
            if (Culprit == null)
            {
                Culprit = ex.TargetSite == null || ex.TargetSite.ReflectedType == null
                    ? null
                    : $"{ex.TargetSite.ReflectedType.FullName} in {ex.TargetSite.Name}";
            }

            if (Message == null)
            {
                Message = ex.Message;
            }

            Exception = ConvertException(ex);
        }
        
        private static List<ISentryException> ConvertException(Exception ex)
        {
            var sentryException = new SentryException
            {
                Module = ex.Source,
                Stacktrace = SentryStacktrace.FromException(ex),
                Type = ex.GetType().FullName,
                Value = ex.Message
            };

            return new[] { sentryException }
                .Concat(ex.InnerException == null ? new List<ISentryException>() : ConvertException(ex.InnerException))
                .ToList();
        }

        /// <summary>
        /// Builds an event with the current data in this builder. If you want to create and send an event immediately,
        /// use <see cref="CaptureAsync"/> instead.
        /// </summary>
        public SentryEvent Build()
        {
            return new SentryEvent
            {
                EventId = (EventId ?? Guid.NewGuid()).ToString("N"),
                Timestamp = Timestamp ?? DateTime.UtcNow,
                Logger = Logger,
                Platform = Platform,
                Sdk = Sdk,
                Level = Level ?? (Exception == null ? SeverityLevel.Info : SeverityLevel.Error),
                Culprit = Culprit,
                ServerName = ServerName,
                Release = Release,
                Tags = Tags.Any() ? Tags.ToDictionary(p => p.Key, p => p.Value) : null,
                Environment = Environment,
                Modules = Modules.Any() ? Modules : null,
                Extra = Extra,
                Fingerprint = Fingerprint.Any() ? Fingerprint : null,
                Exception = Exception != null && Exception.Any() ? Exception : null,
                Message = Message,
                Breadcrumbs = Breadcrumbs,
                Request = Request,
                User = User,
                Contexts = Contexts.Any() ? Contexts.ToDictionary(p => p.Key, p => p.Value) : null
            };
        }

        /// <summary>
        /// Creates an event the current data in this builder and sends it to Sentry.
        /// </summary>
        public async Task<string> CaptureAsync()
        {
            return await _client.SendAsync(Build());
        }
    }
}
