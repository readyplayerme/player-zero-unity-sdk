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
    
    public class AvatarSessionEndedProperties : IGameSession, IGame, IEventContext
    {
        [JsonProperty("avatar_session_id")]
        public string SessionId { get; set; }
        
        [JsonProperty("game_id")]
        public string GameId { get; set; }
        
        [JsonProperty("device_id")]
        public string DeviceId { get; set; }
        
        [JsonProperty("sdk_version")]
        public string SdkVersion { get; set; }
    }
}