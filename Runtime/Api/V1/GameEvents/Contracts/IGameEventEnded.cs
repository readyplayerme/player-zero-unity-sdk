namespace PlayerZero.Api.V1.Contracts
{
    /// <summary>
    /// Represents a game event that marks the end of a game session.
    /// Inherits session properties and authentication token from <see cref="IGameEvent{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of session properties, implementing <see cref="IGameSession"/>.</typeparam>
    public interface IGameEventEnded<T> : IGameEvent<T> where T : class, IGameSession
    {
    }

}