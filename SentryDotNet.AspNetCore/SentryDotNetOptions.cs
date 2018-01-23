using System;

using Microsoft.AspNetCore.Http;

namespace SentryDotNet.AspNetCore
{
    public class SentryDotNetOptions
    {
        public bool CaptureRequestBody { get; set; } = false;

        public Func<HttpRequest, object> CaptureRequestBodyConverter { get; set; } = SentryDotNetBodyConverters.ConvertJsonBody;
    }
}