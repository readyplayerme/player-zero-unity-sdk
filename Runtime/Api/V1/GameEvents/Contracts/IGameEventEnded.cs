namespace PlayerZero.Api.V1.Contracts
{
    public interface IGameEventEnded<T> : IGameEvent<T> where T : class, IGameEventProperties
    {
        
    }
}