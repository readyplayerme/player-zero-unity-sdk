namespace PlayerZero.Api.V1.Contracts
{
    public interface IEventContext
    {
        public string DeviceId { get; set; }
        public string SdkVersion { get; set; }
    }
}
