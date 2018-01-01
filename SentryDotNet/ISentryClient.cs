using System;
using System.Threading.Tasks;

namespace SentryDotNet
{
    /// <summary>
    /// The Sentry client is the main interaction point for creating events / event builders and sending them to Sentry.
    /// Note that the Sentry client itself does not have mutual state and is thread safe. It can (and should) be shared
    /// accross multiple requests/threads/sessions.
    /// </summary>
    public interface ISentryClient
    {
        Dsn Dsn { get; }
        
        /// <summary>
        /// Sends the given <paramref name="sentryEvent"/> to Sentry. Use <see cref="CreateEventBuilder"/> to create a 
        /// customized event. This method does not add any additional information to the event, it is sent as-is.
        /// </summary>
        /// <returns>The ID that Sentry assigned to the event on successful reception. If the event was not sent to
        /// Sentry due to an empty <see cref="Dsn"/> or the sampling mechanism, an empty string is returned.</returns>
        /// <exception cref="SentryClientException">Thrown when Sentry returned a non-success status code in response
        /// to the event. The status code and message are available on the exception.</exception>
        Task<string> SendAsync(SentryEvent sentryEvent);
        
        /// <summary>
        /// Convenience method to capture an exception and send it to Sentry with severity "error". This does not 
        /// capture additional information such as breadcrumbs, use <see cref="CreateEventBuilder"/> if you need that 
        /// functionality.
        /// </summary>
        Task<string> CaptureAsync(Exception exception);
        
        /// <summary>
        /// Convenience method to capture a message and send it to Sentry with severity "info". This does not capture 
        /// additional information  such as breadcrumbs, use <see cref="CreateEventBuilder"/> if you need that 
        /// functionality.
        /// </summary>
        Task<string> CaptureAsync(FormattableString message);
        
        /// <summary>
        /// Convenience method to capture a message and send it to Sentry with severity "info". This does not capture 
        /// additional information  such as breadcrumbs, use <see cref="CreateEventBuilder"/> if you need that 
        /// functionality.
        /// </summary>
        Task<string> CaptureAsync(object message);
        
        /// <summary>
        /// Creates an event builder to capture additional data in an event. Note that the event builder holds a reference
        /// to the client, so the event builder can be used to send the event directly.
        /// </summary>
        SentryEventBuilder CreateEventBuilder();
    }
}