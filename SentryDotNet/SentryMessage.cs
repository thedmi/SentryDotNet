namespace SentryDotNet
{
    public class SentryMessage : ISentryMessage
    {
        public string Formatted => Params == null || Params.Length == 0 ? Message : string.Format(Message, Params);

        public string Message { get; set; }

        public object[] Params { get; set; }
    }
}