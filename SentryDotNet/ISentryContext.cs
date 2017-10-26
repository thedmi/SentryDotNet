namespace SentryDotNet
{
    /// <summary>
    /// The context interfaces provide additional context data. Typically this is data related to the current user, the current
    /// HTTP request.
    /// <para />
    /// https://docs.sentry.io/clientdev/interfaces/contexts/
    /// </summary>
    public interface ISentryContext
    {
        string Type { get; }
    }
}