﻿using System.Collections.Generic;

namespace SentryDotNet
{
    public class HttpSentryContext : IHttpSentryContext
    {
        public string Url { get; set; }
        
        public string Method { get; set; }
        
        public object Data { get; set; }
        
        public string QueryString { get; set; }
        
        public string Cookies { get; set; }
        
        public Dictionary<string, string> Headers { get; set; }
        
        public Dictionary<string, string> Env { get; set; }
    }
}