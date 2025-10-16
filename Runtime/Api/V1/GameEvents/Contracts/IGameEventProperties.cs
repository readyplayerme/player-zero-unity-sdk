namespace PlayerZero.Api.V1.Contracts
{
    /// <summary>
    /// Represents a game session with a unique session identifier.
    /// Used to associate events or data with a specific session.
    /// </summary>
    public interface IGameSession
    {
        /// <summary>
        /// The unique identifier for the game session.
        /// </summary>
        string SessionId { get; set; }
    }

    /// <summary>
    /// Represents a game with a unique game identifier.
    /// Used to associate events or data with a specific game.
    /// </summary>
    public interface IGame
    {
        /// <summary>
        /// The unique identifier for the game.
        /// </summary>
        string GameId { get; set; }
    }
}