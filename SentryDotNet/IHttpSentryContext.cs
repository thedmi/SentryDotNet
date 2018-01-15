namespace SentryDotNet
{
    public interface IHttpSentryContext : ISentryContext
    {
        string Url { get; set; }

        string Method { get; set; }
    }
}