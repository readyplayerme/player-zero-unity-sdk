using Newtonsoft.Json;
using PlayerZero.Api.V1.Contracts;

namespace PlayerZero.Api.V1
{
    public class AvatarSessionHeartbeatEvent : IGameEvent<AvatarSessionHeartbeatProperties>
    {
        [JsonProperty("event")]
        public const string Event = "avatar_session_heartbeat";
        
        [JsonProperty("properties")]
        public AvatarSessionHeartbeatProperties Properties { get; set; }
        
        [JsonProperty("token")]
        public string Token { get; set; }
    }
    
    public class AvatarSessionHeartbeatProperties : IGameSession, IGame
    {
        [JsonProperty("avatar_session_id")]
        public string SessionId { get; set; }
        
        [JsonProperty("game_id")]
        public string GameId { get; set; }
        [JsonProperty("last_avatar_activity_at")]
        public long LastAvatarActivityAt { get; set; }
    }
}