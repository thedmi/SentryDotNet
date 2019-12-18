using System;
using System.IO;
using System.Text;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;

using Newtonsoft.Json.Linq;

namespace SentryDotNet.AspNetCore
{
    public static class SentryDotNetBodyConverters
    {
        public static object ConvertJsonBody(HttpRequest request)
        {
            if (request.ContentType == null || request.ContentType.ToLowerInvariant() != "application/json")
            {
                return null;
            }

            // Allows using several time the stream in ASP.Net Core
            request.EnableBuffering();

            try
            {
                using (var reader = new StreamReader(request.Body, Encoding.UTF8, true, 1024, true))
                {
                    var json = reader.ReadToEnd();
                    return string.IsNullOrWhiteSpace(json) ? null : JObject.Parse(json);
                }
            }
            catch (Exception)
            {
                // Intentionally catch all errors that occur during body reading, so that malformed body content
                // can't trigger failures.
                return null;
            }
            finally
            {
                // Rewind, so the core is not lost when it looks the body for the request
                request.Body.Position = 0;
            }
        }
    }
}