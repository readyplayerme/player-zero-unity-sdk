using Newtonsoft.Json;
using PlayerZero.Api.V1.Contracts;

namespace PlayerZero.Api.V1
{
    public class AvatarSessionStartedEvent : IGameEventStarted<AvatarSessionStartedProperties>
    {
        [JsonProperty("event")]
        public const string Event = "avatar_session_started";
        
        [JsonProperty("properties")]
        public AvatarSessionStartedProperties Properties { get; set; }
        
        [JsonProperty("token")]
        public string Token { get; set; }
    }
    
    public class AvatarSessionStartedProperties : IGameSession, IGame
    {
        [JsonProperty("avatar_id")]
        public string AvatarId { get; set; }

        [JsonProperty("avatar_type")]
        public string AvatarType { get; set; } = "fullbody";

        [JsonProperty("game_id")]
        public string GameId { get; set; }
        
        [JsonProperty("game_session_id")]
        public string GameSessionId { get; set; }
        
        [JsonProperty("avatar_session_id")]
        public string SessionId { get; set; }
        
        [JsonProperty("sdk_version")]
        public string SdkVersion { get; set; }
        
        [JsonProperty("sdk_platform")]
        public string SdkPlatform { get; set; }
                
        [JsonProperty("device_context")]
        public DeviceContext DeviceContext { get; set; }
    }
}