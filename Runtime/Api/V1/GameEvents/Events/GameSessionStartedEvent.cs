using Newtonsoft.Json;
using PlayerZero.Api.V1.Contracts;

namespace PlayerZero.Api.V1
{
    public class GameSessionStartedEvent : IGameEventStarted<GameSessionStartedProperties>
    {
        [JsonProperty("event")]
        public const string Event = "user_game_session_started";
        
        [JsonProperty("properties")]
        public GameSessionStartedProperties Properties { get; set; }
    }
    
    public class GameSessionStartedProperties : IGameSession, IGame, IEventContext
    {
        [JsonProperty("avatar_id")]
        public string AvatarId { get; set; }

        [JsonProperty("application_id")]
        public string ApplicationId { get; set; }

        [JsonProperty("product_name")]
        public string ProductName { get; set; }
        
        [JsonProperty("game_id")]
        public string GameId { get; set; }
        
        [JsonProperty("game_session_id")]
        public string SessionId { get; set; }

        [JsonProperty("game_entry_point")]
        public string GameEntryPoint { get; set; }

        [JsonProperty("live_ops_id")]
        public string LiveOpsId { get; set; }
        
        [JsonProperty("device_id")]
        public string DeviceId { get; set; }
        
        [JsonProperty("sdk_version")]
        public string SdkVersion { get; set; }
    }
}