using Newtonsoft.Json;

namespace PlayerZero.Api.V3
{
    /// <summary>
    /// Represents the response from an avatar code API call.
    /// Inherits from <see cref="ApiResponse"/> and contains avatar code data.
    /// </summary>
    public class AvatarCodeResponse : ApiResponse
    {
        /// <summary>
        /// The data payload containing avatar code information.
        /// </summary>
        [JsonProperty("data")]
        public AvatarCodeData Data { get; set; }
    }

    /// <summary>
    /// Encapsulates the avatar identifier and code returned in the API response.
    /// </summary>
    public class AvatarCodeData
    {
        /// <summary>
        /// The unique identifier for the avatar.
        /// </summary>
        [JsonProperty("avatarId")]
        public string AvatarId { get; set; }

        /// <summary>
        /// The code associated with the avatar.
        /// </summary>
        [JsonProperty("code")]
        public string Code { get; set; }
    }
}
