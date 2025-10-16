using Newtonsoft.Json;
using PlayerZero.Api.V1.Contracts;

namespace PlayerZero.Api.V1
{
    /// <summary>
    /// Represents an event indicating that a game session has started.
    /// Implements <see cref="IGameEventStarted{T}"/> with session and game properties.
    /// </summary>
    public class GameSessionStartedEvent : IGameEventStarted<GameSessionStartedProperties>
    {
        /// <summary>
        /// The event name identifier for a started game session.
        /// </summary>
        [JsonProperty("event")]
        public const string Event = "user_game_session_started";

        /// <summary>
        /// The properties associated with the started game session.
        /// </summary>
        [JsonProperty("properties")]
        public GameSessionStartedProperties Properties { get; set; }

        /// <summary>
        /// The authentication token for the event.
        /// </summary>
        public string Token { get; set; }
    }
    
    /// <summary>
    /// Contains properties related to a started game session event.
    /// Implements <see cref="IGameSession"/> and <see cref="IGame"/> for session and game identification.
    /// </summary>
    public class GameSessionStartedProperties : IGameSession, IGame
    {
        /// <summary>
        /// The unique identifier for the avatar.
        /// </summary>
        [JsonProperty("avatar_id")]
        public string AvatarId { get; set; }

        /// <summary>
        /// The unique identifier for the application.
        /// </summary>
        [JsonProperty("application_id")]
        public string ApplicationId { get; set; }

        /// <summary>
        /// The product name associated with the session.
        /// </summary>
        [JsonProperty("product_name")]
        public string ProductName { get; set; }

        /// <summary>
        /// The unique identifier for the game.
        /// </summary>
        [JsonProperty("game_id")]
        public string GameId { get; set; }

        /// <summary>
        /// The unique identifier for the game session.
        /// </summary>
        [JsonProperty("game_session_id")]
        public string SessionId { get; set; }

        /// <summary>
        /// The entry point used to start the game.
        /// </summary>
        [JsonProperty("game_entry_point")]
        public string GameEntryPoint { get; set; }

        /// <summary>
        /// The live operations identifier for the session.
        /// </summary>
        [JsonProperty("live_ops_id")]
        public string LiveOpsId { get; set; }
    }
}