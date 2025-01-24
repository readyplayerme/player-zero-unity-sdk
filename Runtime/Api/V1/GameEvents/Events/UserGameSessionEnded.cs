using System.Collections.Generic;
using Newtonsoft.Json;

namespace PlayerZero.Api.V1
{
    public class UserGameSessionEnded
    {
        [JsonProperty("event")]
        public const string Event = "user_game_session_ended";
        [JsonProperty("properties")]
        public UserGameSessionEndedProperties Properties { get; set; }
    }
    
    public class UserGameSessionEndedProperties 
    {
        [JsonProperty("user_id")]
        public string UserId { get; set; }
        [JsonProperty("game_session_id")]
        public string GameSessionId { get; set; }
        [JsonProperty("matches_played")]
        public int MatchesPlayed { get; set; }
        [JsonProperty("matches_won")]
        public int MatchesWon { get; set; }
        [JsonProperty("score")]
        public int Score { get; set; }
        [JsonProperty("currency_obtained")]
        public Dictionary<string, object> CurrencyObtained { get; set; } = new Dictionary<string, object>();
    }
}