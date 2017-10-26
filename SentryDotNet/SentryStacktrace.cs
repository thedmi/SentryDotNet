using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SentryDotNet
{
    /// <summary>
    /// A stacktrace contains a list of frames.
    /// <para />
    /// https://docs.sentry.io/clientdev/interfaces/stacktrace/
    /// </summary>
    public class SentryStacktrace
    {
        public static SentryStacktrace FromException(Exception exception)
        {
            return new SentryStacktrace
            {
                Frames = new List<ISentryStacktraceFrame>(new StackTrace(exception, true)
                    .GetFrames()
                    .Reverse()
                    .Select(frame => new SentryStacktraceFrame(frame))
                    .ToList())
            };
        }

        /// <summary>
        /// A list of <see cref="ISentryStacktraceFrame" />.
        /// </summary>
        public IReadOnlyList<ISentryStacktraceFrame> Frames { get; set; }
    }
}