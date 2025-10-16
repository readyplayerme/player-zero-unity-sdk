using Newtonsoft.Json;
using PlayerZero.Api.V1.Contracts;

namespace PlayerZero.Api.V1
{
    /// <summary>
    /// Represents an event indicating that an avatar session has started.
    /// Implements <see cref="IGameEventStarted{T}"/> with session and game properties.
    /// </summary>
    public class AvatarSessionStartedEvent : IGameEventStarted<AvatarSessionStartedProperties>
    {
        /// <summary>
        /// The event name identifier for a started avatar session.
        /// </summary>
        [JsonProperty("event")]
        public const string Event = "avatar_session_started";

        /// <summary>
        /// The properties associated with the started avatar session.
        /// </summary>
        [JsonProperty("properties")]
        public AvatarSessionStartedProperties Properties { get; set; }

        /// <summary>
        /// The authentication token for the event.
        /// </summary>
        [JsonProperty("token")]
        public string Token { get; set; }
    }
    
    /// <summary>
    /// Contains properties related to a started avatar session event.
    /// Implements <see cref="IGameSession"/> and <see cref="IGame"/> for session and game identification.
    /// </summary>
    public class AvatarSessionStartedProperties : IGameSession, IGame
    {
        /// <summary>
        /// The unique identifier for the avatar.
        /// </summary>
        [JsonProperty("avatar_id")]
        public string AvatarId { get; set; }

        /// <summary>
        /// The type of avatar (e.g., fullbody).
        /// </summary>
        [JsonProperty("avatar_type")]
        public string AvatarType { get; set; } = "fullbody";

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
        /// The unique identifier for the avatar session.
        /// </summary>
        [JsonProperty("avatar_session_id")]
        public string SessionId { get; set; }

        /// <summary>
        /// The version of the SDK in use.
        /// </summary>
        [JsonProperty("sdk_version")]
        public string SdkVersion { get; set; }

        /// <summary>
        /// The platform of the SDK in use.
        /// </summary>
        [JsonProperty("sdk_platform")]
        public string SdkPlatform { get; set; }

        /// <summary>
        /// The device context for the session.
        /// </summary>
        [JsonProperty("device_context")]
        public DeviceContext DeviceContext { get; set; }
    }
}