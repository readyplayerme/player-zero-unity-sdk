using Newtonsoft.Json;

namespace PlayerZero.Api.V1
{
    /// <summary>
    /// Represents a request to log in using a code and application ID via the API.
    /// Used for serializing login requests with a one-time code.
    /// </summary>
    public class LoginWithCodeRequest
    {
        /// <summary>
        /// The one-time code provided for authentication.
        /// </summary>
        [JsonProperty("code")]
        public string Code;

        /// <summary>
        /// The unique identifier of the application.
        /// </summary>
        [JsonProperty("appId")]
        public string AppId;
    }

}
