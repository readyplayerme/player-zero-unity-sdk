using System.Collections.Generic;
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

    public class GameMatchEndedProperties : IGameSession, IGame
    {
        [JsonProperty("game_match_id")]
        public string SessionId { get; set; }
        
        [JsonProperty("outcome")]
        public int? Outcome { get; set; }
        
        [JsonProperty("score")]
        public int? Score { get; set; }

        [JsonProperty("currency_obtained")]
        public Dictionary<string, object> CurrencyObtained { get; set; } = new Dictionary<string, object>();
    }
}