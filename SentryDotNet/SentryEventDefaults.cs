using System.Collections.Generic;

namespace SentryDotNet
{
    public class SentryEventDefaults
    {
        public SentryEventDefaults(
            string logger = null,
            SeverityLevel? level = null,
            string serverName = null,
            string release = null,
            IReadOnlyDictionary<string, string> tags = null,
            string environment = null,
            IReadOnlyDictionary<string, string> modules = null,
            object extra = null,
            IReadOnlyDictionary<string, ISentryContext> contexts = null)
        {
            Logger = logger;
            Level = level;
            ServerName = serverName;
            Release = release;
            Tags = tags;
            Environment = environment;
            Modules = modules;
            Extra = extra;
            Contexts = contexts;
        }

        public string Logger { get; }

        public SeverityLevel? Level { get; }

        public string ServerName { get; }

        public string Release { get; }

        public IReadOnlyDictionary<string, string> Tags { get; }

        public string Environment { get; }

        public IReadOnlyDictionary<string, string> Modules { get; }

        public object Extra { get; }

        public IReadOnlyDictionary<string, ISentryContext> Contexts { get; }

    }
}