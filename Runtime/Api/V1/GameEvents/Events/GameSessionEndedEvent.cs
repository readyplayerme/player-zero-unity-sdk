using System.Collections.Generic;
using Newtonsoft.Json;
using PlayerZero.Api.V1.Contracts;

namespace PlayerZero.Api.V1
{
    /// <summary>
    /// Represents an event indicating that a game session has ended.
    /// Implements <see cref="IGameEventEnded{T}"/> with session and game properties.
    /// </summary>
    public class GameSessionEndedEvent : IGameEventEnded<GameSessionEndedProperties>
    {
        /// <summary>
        /// The event name identifier for an ended game session.
        /// </summary>
        [JsonProperty("event")]
        public const string Event = "user_game_session_ended";

        /// <summary>
        /// The properties associated with the ended game session.
        /// </summary>
        [JsonProperty("properties")]
        public GameSessionEndedProperties Properties { get; set; }

        /// <summary>
        /// The authentication token for the event.
        /// </summary>
        [JsonProperty("token")]
        public string Token { get; set; }
    }
    
    /// <summary>
    /// Contains properties related to an ended game session event.
    /// Implements <see cref="IGameSession"/> and <see cref="IGame"/> for session and game identification.
    /// </summary>
    public class GameSessionEndedProperties : IGameSession, IGame
    {
        /// <summary>
        /// The unique identifier for the game session.
        /// </summary>
        [JsonProperty("game_session_id")]
        public string SessionId { get; set; }

        /// <summary>
        /// The number of matches played during the session.
        /// </summary>
        [JsonProperty("matches_played")]
        public int? MatchesPlayed { get; set; }

        /// <summary>
        /// The number of matches won during the session.
        /// </summary>
        [JsonProperty("matches_won")]
        public int? MatchesWon { get; set; }

        /// <summary>
        /// The total score achieved in the session.
        /// </summary>
        [JsonProperty("score")]
        public int? Score { get; set; }

        /// <summary>
        /// The currencies obtained during the session, keyed by currency type.
        /// </summary>
        [JsonProperty("currency_obtained")]
        public Dictionary<string, object> CurrencyObtained { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// The unique identifier for the game.
        /// </summary>
        [JsonProperty("game_id")]
        public string GameId { get; set; }
    }
}