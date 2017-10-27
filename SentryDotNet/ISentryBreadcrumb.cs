using System;

namespace SentryDotNet
{
    /// <summary>
    /// A breadcrumb is a event that happened prior to a event. Multiple breadcrumbs can be used to describe the sequence of
    /// events that triggered a event.
    /// </summary>
    public interface ISentryBreadcrumb
    {
        /// <summary>
        /// Categories are dotted strings that indicate what the crumb is or where it comes from.
        /// Typically it’s a module name or a descriptive string.
        /// For instance ui.click could be used to indicate that a click happend in the UI or flask could be used to indicate that
        /// the event originated in the Flask framework.
        /// </summary>
        string Category { get; }

        /// <summary>
        /// Data associated with this breadcrumb. Contains a sub-object whose contents depend on the <see cref="BreadcrumbType" />.
        /// Additional parameters that are unsupported by the type are rendered as a key/value table.
        /// </summary>
        object Data { get; }

        /// <summary>
        /// This defines the level of the event.
        /// If not provided it defaults to <see cref="SeverityLevel.Info" /> which is the middle level.
        /// In the order of priority from highest to lowest the levels are <see cref="SeverityLevel.Fatal" />,
        /// <see cref="SeverityLevel.Error" />, <see cref="SeverityLevel.Warning" />, <see cref="SeverityLevel.Info" /> and
        /// <see cref="SeverityLevel.Debug" />.
        /// Levels are used in the UI to emphasize and deemphasize the crumb.
        /// </summary>
        SeverityLevel Level { get; }

        /// <summary>
        /// If a message is provided it’s rendered as text and the whitespace is preserved. Very long text might be abbreviated in
        /// the UI.
        /// </summary>
        string Message { get; }

        /// <summary>
        /// A timestamp representing when the breadcrumb occurred. This can be either an ISO datetime string, or a Unix timestamp.
        /// </summary>
        DateTime Timestamp { get; }

        /// <summary>
        /// The type of breadcrumb. The default type is default which indicates no specific handling.
        /// Other types are currently http for HTTP requests and navigation for navigation events.
        /// </summary>
        BreadcrumbType Type { get; }
    }
}