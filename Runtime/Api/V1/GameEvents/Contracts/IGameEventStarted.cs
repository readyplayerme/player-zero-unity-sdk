namespace PlayerZero.Api.V1.Contracts
{
    public interface IGameEventStarted<T> : IGameEvent<T> where T : class, IGameSession, IGame
    {
        
    }
}