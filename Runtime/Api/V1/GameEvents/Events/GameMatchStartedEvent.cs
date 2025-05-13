using System.Collections.Generic;
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
        
        [JsonProperty("token")]
        public string Token { get; set; }
    }
    
    public class GameMatchStartedProperties : IGameSession, IGame
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
        public string GameSessionId { get; set; }
        
        [JsonProperty("game_match_id")]
        public string SessionId { get; set; }

        [JsonProperty("start_context")]
        public string StartContext { get; set; }

        [JsonProperty("live_ops_id")]
        public string LiveOpsId { get; set; }
        
        [JsonProperty("tier")]
        public int? Tier { get; set; }
        
        [JsonProperty("round")]
        public int? Round { get; set; }

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
        
        [JsonProperty("lobby_id")]
        public string LobbyId { get; set; }
    }
}