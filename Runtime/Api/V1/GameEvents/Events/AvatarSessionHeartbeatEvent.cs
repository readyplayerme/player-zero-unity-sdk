using Newtonsoft.Json;
using PlayerZero.Api.V1.Contracts;

namespace PlayerZero.Api.V1
{
    /// <summary>
    /// Represents a heartbeat event for an active avatar session.
    /// Implements <see cref="IGameEvent{T}"/> with session and game properties.
    /// </summary>
    public class AvatarSessionHeartbeatEvent : IGameEvent<AvatarSessionHeartbeatProperties>
    {
        /// <summary>
        /// The event name identifier for an avatar session heartbeat.
        /// </summary>
        [JsonProperty("event")]
        public const string Event = "avatar_session_heartbeat";
        
        ///     /// <summary>
        /// The properties associated with the avatar session heartbeat.
        /// </summary>
        [JsonProperty("properties")]
        public AvatarSessionHeartbeatProperties Properties { get; set; }
        
        /// <summary>
        /// The authentication token for the event.
        /// </summary>
        [JsonProperty("token")]
        public string Token { get; set; }
    }
    
    /// <summary>
    /// Contains properties related to a heartbeat event for an active avatar session.
    /// Implements <see cref="IGameSession"/> and <see cref="IGame"/> for session and game identification.
    /// </summary>
    public class AvatarSessionHeartbeatProperties : IGameSession, IGame
    {
        /// <summary>
        /// The unique identifier for the avatar session.
        /// </summary>
        [JsonProperty("avatar_session_id")]
        public string SessionId { get; set; }
        
        /// <summary>
        /// The unique identifier for the game.
        /// </summary>
        [JsonProperty("game_id")]
        public string GameId { get; set; }
        
        /// <summary>
        /// The timestamp of the last avatar activity, in Unix epoch milliseconds.
        /// </summary>
        [JsonProperty("last_avatar_activity_at")]
        public long LastAvatarActivityAt { get; set; }
    }
}