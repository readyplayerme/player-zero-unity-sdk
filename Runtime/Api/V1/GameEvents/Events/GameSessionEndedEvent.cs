using System.Collections.Generic;
using Newtonsoft.Json;
using PlayerZero.Api.V1.Contracts;

namespace PlayerZero.Api.V1
{
    public class GameSessionEndedEvent : IGameEventEnded<GameSessionEndedProperties>
    {
        [JsonProperty("event")]
        public const string Event = "user_game_session_ended";
        
        [JsonProperty("properties")]
        public GameSessionEndedProperties Properties { get; set; }
    }
    
    public class GameSessionEndedProperties : IGameSession, IGame, IEventContext
    {
        [JsonProperty("game_session_id")]
        public string SessionId { get; set; }
        
        [JsonProperty("matches_played")]
        public int? MatchesPlayed { get; set; }
        
        [JsonProperty("matches_won")]
        public int? MatchesWon { get; set; }
        
        [JsonProperty("score")]
        public int? Score { get; set; }

        [JsonProperty("currency_obtained")]
        public Dictionary<string, object> CurrencyObtained { get; set; } = new Dictionary<string, object>();
        
        [JsonProperty("game_id")]
        public string GameId { get; set; }
        
        [JsonProperty("device_id")]
        public string DeviceId { get; set; }
        
        [JsonProperty("sdk_version")]
        public string SdkVersion { get; set; }
    }
}