using System.Collections.Generic;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace PlayerZero.Api.V1
{
    public class UserGameMatchEndedEvent
    {
        [JsonProperty("event")]
        public const string Event = "user_game_match_ended";
        
        [JsonProperty("properties")]
        public UserGameMatchEndedProperties Properties { get; set; }
    }

    public class UserGameMatchEndedProperties
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
    }
}