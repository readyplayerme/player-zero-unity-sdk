using System.Collections.Generic;
using JetBrains.Annotations;
using Newtonsoft.Json;
using PlayerZero.Api.V1.Contracts;

namespace PlayerZero.Api.V1
{
    public class GameMatchEndedEvent : IGameEventEnded<GameMatchEndedProperties>
    {
        [JsonProperty("event")]
        public const string Event = "user_game_match_ended";
        
        [JsonProperty("properties")]
        public GameMatchEndedProperties Properties { get; set; }
    }

    public class GameMatchEndedProperties : IGameEventProperties
    {
        [JsonProperty("user_id")]
        public string UserId { get; set; }
        
        [JsonProperty("game_match_id")]
        public string GameMatchId { get; set; }
        
        [JsonProperty("outcome")]
        public int? Outcome { get; set; }
        
        [JsonProperty("score")]
        public int? Score { get; set; }

        [JsonProperty("currency_obtained"), CanBeNull]
        public Dictionary<string, object> CurrencyObtained { get; set; } = new Dictionary<string, object>();
        
        [JsonProperty("game_id")]
        public string GameId { get; set; }
        
        [JsonProperty("game_session_id")]
        public string SessionId { get; set; }
    }
}