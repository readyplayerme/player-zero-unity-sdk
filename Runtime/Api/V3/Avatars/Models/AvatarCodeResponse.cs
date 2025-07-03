using Newtonsoft.Json;

namespace PlayerZero.Api.V3
{
    public class AvatarCodeResponse : ApiResponse
    {
        [JsonProperty("data")]
        public AvatarCodeData Data { get; set; }
    }

    public class AvatarCodeData
    {
        [JsonProperty("avatarId")]
        public string AvatarId { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }
    }
}
