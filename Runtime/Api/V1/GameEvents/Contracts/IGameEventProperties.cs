namespace PlayerZero.Api.V1.Contracts
{
    public interface IGameEventProperties
    {
        public string GameId { get; set; }
        
        public string SessionId { get; set; }
    }
}