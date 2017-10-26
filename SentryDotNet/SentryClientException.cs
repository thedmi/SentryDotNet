using System;

namespace SentryDotNet
{
    [Serializable]
    public class SentryClientException : Exception
    {
        public SentryClientException()
        {
        }

        public SentryClientException(string message) : base(message)
        {
        }

        public SentryClientException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}