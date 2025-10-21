using System.Collections.Generic;
using Newtonsoft.Json;
using PlayerZero.Api.V1.Contracts;

namespace PlayerZero.Api.V1
{
    /// <summary>
    /// Represents an event indicating that a game match has started.
    /// Implements <see cref="IGameEventStarted{T}"/> with match and game properties.
    /// </summary>
    public class GameMatchStartedEvent : IGameEventStarted<GameMatchStartedProperties>
    {
        /// <summary>
        /// The event name identifier for a started game match.
        /// </summary>
        [JsonProperty("event")]
        public const string Event = "user_game_match_started";

        /// <summary>
        /// The properties associated with the started game match.
        /// </summary>
        [JsonProperty("properties")]
        public GameMatchStartedProperties Properties { get; set; }

        /// <summary>
        /// The authentication token for the event.
        /// </summary>
        [JsonProperty("token")]
        public string Token { get; set; }
    }

    
    /// <summary>
    /// Contains properties related to a started game match event.
    /// Implements <see cref="IGameSession"/> and <see cref="IGame"/> for session and game identification.
    /// </summary>
    public class GameMatchStartedProperties : IGameSession, IGame
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
        /// The product name associated with the match.
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
        public string GameSessionId { get; set; }

        /// <summary>
        /// The unique identifier for the game match session.
        /// </summary>
        [JsonProperty("game_match_id")]
        public string SessionId { get; set; }

        /// <summary>
        /// The context in which the match was started.
        /// </summary>
        [JsonProperty("start_context")]
        public string StartContext { get; set; }

        /// <summary>
        /// The live operations identifier for the match.
        /// </summary>
        [JsonProperty("live_ops_id")]
        public string LiveOpsId { get; set; }

        /// <summary>
        /// The tier of the match, if applicable.
        /// </summary>
        [JsonProperty("tier")]
        public int? Tier { get; set; }

        /// <summary>
        /// The round number of the match, if applicable.
        /// </summary>
        [JsonProperty("round")]
        public int? Round { get; set; }

        /// <summary>
        /// The unique identifier for the map used in the match.
        /// </summary>
        [JsonProperty("map_id")]
        public string MapId { get; set; }

        /// <summary>
        /// The game mode for the match.
        /// </summary>
        [JsonProperty("game_mode")]
        public string GameMode { get; set; }

        /// <summary>
        /// The class or role of the player in the match.
        /// </summary>
        [JsonProperty("class")]
        public string Class { get; set; }

        /// <summary>
        /// The team identifier for the player in the match.
        /// </summary>
        [JsonProperty("team")]
        public string Team { get; set; }

        /// <summary>
        /// The loadout configuration for the player, keyed by item type.
        /// </summary>
        [JsonProperty("loadout")]
        public Dictionary<string, object> Loadout { get; set; }

        /// <summary>
        /// The unique identifier for the lobby in which the match was started.
        /// </summary>
        [JsonProperty("lobby_id")]
        public string LobbyId { get; set; }
    }
}