using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SentryDotNet
{
    /// <summary>
    /// An event that packages a message or an exception, so that it may transmitted to Sentry.
    /// </summary>
    public class SentryEvent
    {
        /// <summary>
        /// Hexadecimal string representing a uuid4 value.
        /// </summary>
        public string EventId { get; set; }

        /// <summary>
        /// Indicates when the logging record was created.
        /// </summary>
        public DateTime Timestamp { get; set; }

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
        public string Platform { get; set; }

        /// <summary>
        /// Information about the <see cref="SentrySdk" /> sending the event.
        /// </summary>
        public SentrySdk Sdk { get; set; }

        /// <summary>
        /// The record <see cref="SeverityLevel" />.
        /// </summary>
        public SeverityLevel Level { get; set; }

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
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IReadOnlyDictionary<string, string> Tags { get; set; }

        /// <summary>
        /// The operating environment that the event was created e.g. production, staging.
        /// </summary>
        public string Environment { get; set; }

        /// <summary>
        /// A list of relevant modules and their versions.
        /// </summary>
        public IReadOnlyDictionary<string, string> Modules { get; set; }

        /// <summary>
        /// An arbitrary mapping of additional metadata to store with the event.
        /// </summary>
        public object Extra { get; set; }

        /// <summary>
        /// An array of strings used to dictate the deduplication of this event.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IReadOnlyList<string> Fingerprint { get; set; }

        /// <summary>
        /// A list of <see cref="ISentryException" /> related to this event.
        /// </summary>
        public IReadOnlyList<ISentryException> Exception { get; set; }

        /// <summary>
        /// A user friendly event that conveys the meaning of this event.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// A trail of breadcrumbs, if any, that led up to the event creation.
        /// </summary>
        public IReadOnlyList<ISentryBreadcrumb> Breadcrumbs { get; set; }
        
        /// <summary>
        /// The HTTP request context, if any.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IHttpSentryContext Request { get; set; }
        
        /// <summary>
        /// Information about the user that triggered the event.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IUserSentryContext User { get; set; }

        /// <summary>
        /// A dictionary of <see cref="ISentryContext" /> for this event.
        /// </summary>
        public IReadOnlyDictionary<string, ISentryContext> Contexts { get; set; }
    }
}