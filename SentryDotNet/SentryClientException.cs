using System;
using System.Net;

namespace SentryDotNet
{
    [Serializable]
    public class SentryClientException : Exception
    {
        public SentryClientException()
        {
        }

        public SentryClientException(HttpStatusCode statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }

        public SentryClientException(string message) : base(message)
        {
        }

        public SentryClientException(string message, Exception innerException) : base(message, innerException)
        {
        }
        
        public HttpStatusCode StatusCode { get; }
    }
}