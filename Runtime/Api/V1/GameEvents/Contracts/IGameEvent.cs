using Newtonsoft.Json;

namespace PlayerZero.Api.V1.Contracts
{
    /// <summary>
    /// Represents a generic game event with associated session properties and an authentication token.
    /// Used to encapsulate event data for a specific game session.
    /// </summary>
    /// <typeparam name="T">The type of session properties, implementing <see cref="IGameSession"/>.</typeparam>
    public interface IGameEvent<T> where T : class, IGameSession
    {
        /// <summary>
        /// The properties associated with the game session for this event.
        /// </summary>
        T Properties { get; set; }

        /// <summary>
        /// The authentication token for the event.
        /// </summary>
        string Token { get; set; }
    }

}