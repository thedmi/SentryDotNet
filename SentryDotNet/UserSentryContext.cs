using Newtonsoft.Json;

namespace SentryDotNet
{
    public class UserSentryContext : IUserSentryContext
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }
        
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; }
        
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string IpAddress { get; set; }
        
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Username { get; set; }
    }
}