using System.Collections.Generic;

namespace SentryDotNet
{
    public class HttpSentryContext : IHttpSentryContext
    {
        public string Type => ContextTypes.Http;
        
        public string Url { get; set; }
        
        public string Method { get; set; }
        
        public string Data { get; set; }
        
        public string QueryString { get; set; }
        
        public string Cookies { get; set; }
        
        public Dictionary<string, string> Headers { get; set; }
        
        public Dictionary<string, string> Env { get; set; }
    }
}