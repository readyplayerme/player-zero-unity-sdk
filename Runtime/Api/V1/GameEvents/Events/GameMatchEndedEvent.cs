using System.Collections.Generic;
using Newtonsoft.Json;
using PlayerZero.Api.V1.Contracts;

namespace PlayerZero.Api.V1
{
    /// <summary>
    /// Represents an event indicating that a game match has ended.
    /// Implements <see cref="IGameEventEnded{T}"/> with match and game properties.
    /// </summary>
    public class GameMatchEndedEvent : IGameEventEnded<GameMatchEndedProperties>
    {
        /// <summary>
        /// The event name identifier for an ended game match.
        /// </summary>
        [JsonProperty("event")]
        public const string Event = "user_game_match_ended";

        /// <summary>
        /// The properties associated with the ended game match.
        /// </summary>
        [JsonProperty("properties")]
        public GameMatchEndedProperties Properties { get; set; }

        /// <summary>
        /// The authentication token for the event.
        /// </summary>
        [JsonProperty("token")]
        public string Token { get; set; }
    }


    /// <summary>
    /// Contains properties related to an ended game match event.
    /// Implements <see cref="IGameSession"/> and <see cref="IGame"/> for session and game identification.
    /// </summary>
    public class GameMatchEndedProperties : IGameSession, IGame
    {
        /// <summary>
        /// The unique identifier for the game match session.
        /// </summary>
        [JsonProperty("game_match_id")]
        public string SessionId { get; set; }

        /// <summary>
        /// The outcome of the match (e.g., win/loss).
        /// </summary>
        [JsonProperty("outcome")]
        public int? Outcome { get; set; }

        /// <summary>
        /// The score achieved in the match.
        /// </summary>
        [JsonProperty("score")]
        public int? Score { get; set; }

        /// <summary>
        /// The currencies obtained during the match, keyed by currency type.
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