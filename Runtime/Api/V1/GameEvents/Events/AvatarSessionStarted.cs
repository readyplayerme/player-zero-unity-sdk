using Newtonsoft.Json;

namespace PlayerZero.Api.V1
{
    public class AvatarSessionStartedEvent
    {
        [JsonProperty("event")]
        public const string Event = "avatar_session_started";
        
        [JsonProperty("properties")]
        public AvatarSessionStartedProperties Properties { get; set; }
    }
    
    public class AvatarSessionStartedProperties
    {
        [JsonProperty("user_id")]
        public string UserId { get; set; }
        
        [JsonProperty("avatar_id")]
        public string AvatarId { get; set; }

        [JsonProperty("avatar_type")]
        public string AvatarType { get; set; } = "FullBody";

        [JsonProperty("game_id")]
        public string GameId { get; set; }
        
        [JsonProperty("game_session_id")]
        public string GameSessionId { get; set; }
        
        [JsonProperty("avatar_session_id")]
        public string AvatarSessionId { get; set; }
    }
}