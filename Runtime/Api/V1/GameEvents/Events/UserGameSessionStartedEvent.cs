using JetBrains.Annotations;
using Newtonsoft.Json;

namespace PlayerZero.Api.V1
{
    public class UserGameSessionStartedEvent
    {
        [JsonProperty("event")]
        public const string Event = "user_game_session_started";
        
        [JsonProperty("properties")]
        public UserGameSessionStartedProperties Properties { get; set; }
    }
    
    public class UserGameSessionStartedProperties
    {
        [JsonProperty("user_id")]
        public string UserId { get; set; }
        
        [JsonProperty("avatar_id")]
        public string AvatarId { get; set; }

        [JsonProperty("application_id"), CanBeNull]
        public string ApplicationId { get; set; }

        [JsonProperty("product_name"), CanBeNull]
        public string ProductName { get; set; }
        
        [JsonProperty("game_id")]
        public string GameId { get; set; }
        
        [JsonProperty("game_session_id")]
        public string GameSessionId { get; set; }

        [JsonProperty("game_entry_point"), CanBeNull]
        public string GameEntryPoint { get; set; }

        [JsonProperty("live_ops_id"), CanBeNull]
        public string LiveOpsId { get; set; }
    }
}