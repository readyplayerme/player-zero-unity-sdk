namespace PlayerZero.Api.V1.Contracts
{
    public interface IGameSession
    {
        public string SessionId { get; set; }
    }

    public interface IGame
    {
        public string GameId { get; set; }
    }
}