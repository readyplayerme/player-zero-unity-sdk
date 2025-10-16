using Newtonsoft.Json;

namespace PlayerZero.Api.V1
{
    /// <summary>
    /// Represents a request to refresh an authentication token via the API.
    /// Contains the payload with the current token and refresh token.
    /// </summary>
    public class RefreshTokenRequest
    {
        /// <summary>
        /// The payload containing the current token and refresh token.
        /// </summary>
        public RefreshTokenRequestBody Payload { get; set; } = new RefreshTokenRequestBody();
    }

    /// <summary>
    /// Contains the current authentication token and the refresh token required for token renewal.
    /// Used for serializing refresh token requests.
    /// </summary>
    public class RefreshTokenRequestBody
    {
        /// <summary>
        /// The current authentication token to be refreshed.
        /// </summary>
        [JsonProperty("token")]
        public string Token { get; set; }

        /// <summary>
        /// The refresh token used to obtain a new authentication token.
        /// </summary>
        [JsonProperty("refreshToken")]
        public string RefreshToken { get; set; }
    }

}