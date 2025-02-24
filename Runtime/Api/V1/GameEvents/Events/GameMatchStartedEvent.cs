using System.Collections.Generic;
using JetBrains.Annotations;
using Newtonsoft.Json;
using PlayerZero.Api.V1.Contracts;

namespace PlayerZero.Api.V1
{
    public class GameMatchStartedEvent : IGameEventStarted<GameMatchStartedProperties>
    {
        [JsonProperty("event")]
        public const string Event = "user_game_match_started";
        
        [JsonProperty("properties")]
        public GameMatchStartedProperties Properties { get; set; }
    }
    
    public class GameMatchStartedProperties : IGameEventProperties
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
        public string SessionId { get; set; }
        
        [JsonProperty("game_match_id")]
        public string GameMatchId { get; set; }

        [JsonProperty("start_context"), CanBeNull]
        public string StartContext { get; set; }

        [JsonProperty("live_ops_id"), CanBeNull]
        public string LiveOpsId { get; set; }
        
        [JsonProperty("tier"), CanBeNull]
        public int Tier { get; set; }
        
        [JsonProperty("round"), CanBeNull]
        public int Round { get; set; }

        [JsonProperty("map_id"), CanBeNull]
        public string MapId { get; set; }

        [JsonProperty("game_mode"), CanBeNull]
        public string GameMode { get; set; }

        [JsonProperty("class"), CanBeNull]
        public string Class { get; set; }

        [JsonProperty("team"), CanBeNull]
        public string Team { get; set; }

        [JsonProperty("loadout"), CanBeNull]
        public Dictionary<string, object> Loadout { get; set; }
    }
}