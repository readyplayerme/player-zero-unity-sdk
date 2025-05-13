using Newtonsoft.Json;

namespace PlayerZero.Api.V1.Contracts
{
    public interface IGameEvent<T> where T : class, IGameSession
    {
        T Properties { get; set; }
        string Token { get; set; }
    }
}