using Newtonsoft.Json;

namespace PlayerZero.Api.V1
{
    /// <summary>
    /// Represents the API response for a login request using a code, containing user details and authentication tokens.
    /// Inherits common response properties from <see cref="ApiResponse"/>.
    /// </summary>
    public class LoginWithCodeResponse : ApiResponse
    {
        /// <summary>
        /// The unique identifier of the authenticated user.
        /// </summary>
        [JsonProperty("_id")]
        public string Id;

        /// <summary>
        /// The email address of the authenticated user.
        /// </summary>
        [JsonProperty("email")]
        public string Email;

        /// <summary>
        /// The display name of the authenticated user.
        /// </summary>
        [JsonProperty("name")]
        public string Name;

        /// <summary>
        /// The authentication token issued for the user.
        /// </summary>
        [JsonProperty("token")]
        public string Token;

        /// <summary>
        /// The refresh token for obtaining new authentication tokens.
        /// </summary>
        [JsonProperty("refreshToken")]
        public string RefreshToken;
    }
}
