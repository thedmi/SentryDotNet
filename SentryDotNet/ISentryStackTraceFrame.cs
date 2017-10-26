namespace SentryDotNet
{
    /// <summary>
    /// A stack trace frame contains various bits (most optional) describing the context of itself.
    /// <para />
    /// https://docs.sentry.io/clientdev/interfaces/stacktrace/
    /// </summary>
    public interface ISentryStacktraceFrame
    {
        /// <summary>
        /// The relative filepath to the call
        /// </summary>
        string Filename { get; set; }

        /// <summary>
        /// The name of the function being called
        /// </summary>
        string Function { get; set; }

        /// <summary>
        /// Platform-specific module path (e.g. sentry.interfaces.Stacktrace)
        /// </summary>
        string Module { get; set; }

        /// <summary>
        /// The line number of the call
        /// </summary>
        int Lineno { get; set; }

        /// <summary>
        /// The column number of the call
        /// </summary>
        int Colno { get; set; }

        /// <summary>
        /// The absolute path to filename
        /// </summary>
        string AbsPath { get; set; }

        /// <summary>
        /// Source code in filename at lineno
        /// </summary>
        string ContextLine { get; set; }

        /// <summary>
        /// A list of source code lines before context_line (in order) – usually [lineno - 5:lineno]
        /// </summary>
        string[] PreContext { get; set; }

        /// <summary>
        /// A list of source code lines after context_line (in order) – usually [lineno + 1:lineno + 5]
        /// </summary>
        string[] PostContext { get; set; }

        /// <summary>
        /// Signifies whether this frame is related to the execution of the relevant code in this stacktrace.
        /// For example, the frames that might power the framework’s webserver of your app are probably not relevant, however calls
        /// to the framework’s library once you start handling code likely are.
        /// </summary>
        bool InApp { get; set; }

        /// <summary>
        /// A mapping of variables which were available within this frame (usually context-locals).
        /// </summary>
        object Vars { get; set; }
    }
}