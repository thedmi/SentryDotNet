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
            request.EnableRewind();

            using (var reader = new StreamReader(request.Body, Encoding.UTF8, true, 1024, true))
            {
                try
                {
                    return JObject.Parse(reader.ReadToEnd());
                }
                finally
                {
                    // Rewind, so the core is not lost when it looks the body for the request
                    request.Body.Position = 0;
                }
            }
        }
    }
}