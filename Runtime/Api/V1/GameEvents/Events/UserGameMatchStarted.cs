using System.Collections.Generic;
using Newtonsoft.Json;

namespace PlayerZero.Api.V1
{
    public class UserGameMatchStarted
    {
        [JsonProperty("name")]
        public const string Name = "user_game_match_started";
        [JsonProperty("properties")]
        public UserGameMatchStartedProperties Properties { get; set; }
    }
    
    public class UserGameMatchStartedProperties
    {
        [JsonProperty("user_id")]
        public string UserId { get; set; }
        [JsonProperty("avatar_id")]
        public string AvatarId { get; set; }
        [JsonProperty("application_id")]
        public string ApplicationId { get; set; }
        [JsonProperty("product_name")]
        public string ProductName { get; set; }
        [JsonProperty("game_id")]
        public string GameId { get; set; }
        [JsonProperty("game_session_id")]
        public string GameSessionId { get; set; }
        [JsonProperty("game_match_id")]
        public string GameMatchId { get; set; }
        [JsonProperty("start_context")]
        public string StartContext { get; set; }
        [JsonProperty("live_ops_id")]
        public string LiveOpsId { get; set; }
        [JsonProperty("tier")]
        public int Tier { get; set; }
        [JsonProperty("round")]
        public int Round { get; set; }
        [JsonProperty("map_id")]
        public string MapId { get; set; }
        [JsonProperty("game_mode")]
        public string GameMode { get; set; }
        [JsonProperty("class")]
        public string Class { get; set; }
        [JsonProperty("team")]
        public string Team { get; set; }
        [JsonProperty("loadout")]
        public Dictionary<string, object> Loadout { get; set; }
    }
}