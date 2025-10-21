using Newtonsoft.Json;
using PlayerZero.Api.V1.Contracts;

namespace PlayerZero.Api.V1
{
    /// <summary>
    /// Represents an event indicating that an avatar session has ended.
    /// Implements <see cref="IGameEventEnded{T}"/> with session and game properties.
    /// </summary>
    public class AvatarSessionEndedEvent : IGameEventEnded<AvatarSessionEndedProperties>
    {
        /// <summary>
        /// The event name identifier for an ended avatar session.
        /// </summary>
        [JsonProperty("event")]
        public const string Event = "avatar_session_ended";
        
        /// <summary>
        /// The properties associated with the ended avatar session.
        /// </summary>
        [JsonProperty("properties")]
        public AvatarSessionEndedProperties Properties { get; set; }
        
        /// <summary>
        /// The authentication token for the event.
        /// </summary>
        [JsonProperty("token")]
        public string Token { get; set; }
    }
    
    /// <summary>
    /// Contains properties related to an ended avatar session event.
    /// Implements <see cref="IGameSession"/> and <see cref="IGame"/> for session and game identification.
    /// </summary>
    public class AvatarSessionEndedProperties : IGameSession, IGame
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
    }
}