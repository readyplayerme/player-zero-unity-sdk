using Newtonsoft.Json;
using PlayerZero.Api.V1.Contracts;

namespace PlayerZero.Api.V1
{
    public class AvatarSessionEndedEvent : IGameEventEnded<AvatarSessionEndedProperties>
    {
        [JsonProperty("event")]
        public const string Event = "avatar_session_ended";
        
        [JsonProperty("properties")]
        public AvatarSessionEndedProperties Properties { get; set; }
    }
    
    public class AvatarSessionEndedProperties : IGameEventProperties
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
        public string SessionId { get; set; }
    }
}