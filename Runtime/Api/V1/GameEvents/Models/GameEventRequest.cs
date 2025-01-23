using Newtonsoft.Json;

namespace PlayerZero.Api.V1
{
    public class GameEventRequest<T>
    {
        [JsonProperty("data")]
        public T Payload { get; set; }
        
        public GameEventRequest(T payload)
        {
            Payload = payload;
        }
    }
}
