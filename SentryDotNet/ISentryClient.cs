using System;
using System.Threading.Tasks;

namespace SentryDotNet
{
    public interface ISentryClient
    {
        Dsn Dsn { get; }
        
        Task<string> SendAsync(SentryEvent sentryEvent);
        
        Task<string> CaptureAsync(Exception exception);
        
        Task<string> CaptureAsync(string message);
        
        SentryEventBuilder CreateEventBuilder();
    }
}