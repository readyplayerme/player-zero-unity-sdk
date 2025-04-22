namespace PlayerZero.Api.V1.Contracts
{
    public interface IGameSession
    {
        string SessionId { get; set; }
    }

    public interface IGame
    {
        string GameId { get; set; }
    }
}