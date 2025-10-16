namespace PlayerZero.Api.V1.Contracts
{
    /// <summary>
    /// Represents a game event that marks the start of a game session.
    /// Inherits session and game properties, as well as authentication token, from <see cref="IGameEvent{T}"/>.
    /// </summary>
    /// <typeparam name="T">
    /// The type of session and game properties, implementing <see cref="IGameSession"/> and <see cref="IGame"/>.
    /// </typeparam>
    public interface IGameEventStarted<T> : IGameEvent<T> where T : class, IGameSession, IGame
    {
    }

}