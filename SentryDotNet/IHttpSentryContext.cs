namespace SentryDotNet
{
    public interface IHttpSentryContext
    {
        string Url { get; set; }

        string Method { get; set; }
    }
}