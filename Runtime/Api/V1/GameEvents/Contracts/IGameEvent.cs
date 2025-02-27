namespace PlayerZero.Api.V1.Contracts
{
    public interface IGameEvent<T> where T : class, IGameSession
    {
        public T Properties { get; set; }
    }
}