using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace SentryDotNet
{
    /// <summary>
    /// A stack trace frame contains various bits (most optional) describing the context of itself.
    /// <para />
    /// https://docs.sentry.io/clientdev/interfaces/stacktrace/
    /// </summary>
    public class SentryStacktraceFrame : ISentryStacktraceFrame
    {
        /// <summary>
        /// Creates a Sentry stacktrace frame using a <see cref="StackFrame" />.
        /// </summary>
        /// <param name="frame">The stack frame used to create the Sentry stacktrace frame.</param>
        public SentryStacktraceFrame (StackFrame frame)
        {
            if (frame == null)
            {
                throw new ArgumentNullException(nameof(frame));
            }

            var method = frame.GetMethod();

            if (method == null)
            {
                Module = "N/A";
                Function = "N/A";
            }
            else
            {
                Function = method.Name;

                if (method.DeclaringType == null)
                {
                    Module = "N/A";
                }
                else
                {
                    Module = method.DeclaringType.FullName;

                    if (Function == "MoveNext")
                    {
                        var match = Regex.Match(Module, @"^(.*)\+<(\w*)>d__\d*$");

                        if (match.Success && match.Groups.Count == 3)
                        {
                            Module = match.Groups[1].Value;
                            Function = match.Groups[2].Value;
                        }
                    }

                    if (Function != null)
                    {
                        var match = Regex.Match(Function, @"^<(\w*)>b__\w+$");

                        if (match.Success && match.Groups.Count == 2)
                        {
                            Function = match.Groups[1].Value + " { <lambda> }";
                        }
                    }
                }
            }

            Lineno = frame.GetFileLineNumber();

            if (Lineno == 0)
            {
                Lineno = frame.GetILOffset();
            }

            Filename = frame.GetFileName();
            Colno = frame.GetFileColumnNumber();
            InApp = !string.IsNullOrWhiteSpace(Module) && Module.StartsWith("System.", StringComparison.OrdinalIgnoreCase) &&
                         Module.StartsWith("Microsoft.", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// The absolute path to filename
        /// </summary>
        public string AbsPath { get; set; }

        /// <summary>
        /// The column number of the call
        /// </summary>
        public int Colno { get; set; }

        /// <summary>
        /// Source code in filename at lineno
        /// </summary>
        public string ContextLine { get; set; }

        /// <summary>
        /// The relative filepath to the call
        /// </summary>
        public string Filename { get; set; }

        /// <summary>
        /// The name of the function being called
        /// </summary>
        public string Function { get; set; }

        /// <summary>
        /// Signifies whether this frame is related to the execution of the relevant code in this stacktrace.
        /// For example, the frames that might power the framework’s webserver of your app are probably not relevant, however calls
        /// to the framework’s library once you start handling code likely are.
        /// </summary>
        public bool InApp { get; set; }

        /// <summary>
        /// The line number of the call
        /// </summary>
        public int Lineno { get; set; }

        /// <summary>
        /// Platform-specific module path (e.g. sentry.interfaces.Stacktrace)
        /// </summary>
        public string Module { get; set; }

        /// <summary>
        /// A list of source code lines after context_line (in order) – usually [lineno + 1:lineno + 5]
        /// </summary>
        public string[] PostContext { get; set; }

        /// <summary>
        /// A list of source code lines before context_line (in order) – usually [lineno - 5:lineno]
        /// </summary>
        public string[] PreContext { get; set; }

        /// <summary>
        /// A mapping of variables which were available within this frame (usually context-locals).
        /// </summary>
        public object Vars { get; set; }
    }
}