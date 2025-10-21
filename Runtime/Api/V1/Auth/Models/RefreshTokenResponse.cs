using Newtonsoft.Json;

namespace PlayerZero.Api.V1
{
    /// <summary>
    /// Represents the API response for a token refresh request, containing the new authentication and refresh tokens.
    /// Inherits common response properties from <see cref="ApiResponse"/>.
    /// </summary>
    public class RefreshTokenResponse : ApiResponse
    {
        /// <summary>
        /// The response body containing the refreshed authentication and refresh tokens.
        /// </summary>
        [JsonProperty("data")]
        public RefreshTokenResponseBody Data { get; set; } = new RefreshTokenResponseBody();
    }

    /// <summary>
    /// Contains the new authentication token and refresh token issued by the API.
    /// Used for deserializing token data from the refresh response.
    /// </summary>
    public class RefreshTokenResponseBody
    {
        /// <summary>
        /// The refreshed authentication token.
        /// </summary>
        [JsonProperty("token")]
        public string Token { get; set; }

        /// <summary>
        /// The new refresh token for future token renewals.
        /// </summary>
        [JsonProperty("refreshToken")]
        public string RefreshToken { get; set; }
    }
}