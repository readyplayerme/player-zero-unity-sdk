using Newtonsoft.Json;

namespace PlayerZero.Api.V1
{
    /// <summary>
    /// Represents a request to create a new user via the API, including the application ID and an option to request a token.
    /// Used for serializing user creation requests.
    /// </summary>
    public class CreateUserRequest
    {
        /// <summary>
        /// The unique identifier of the application for which the user is being created.
        /// </summary>
        [JsonProperty("applicationId")]
        public string ApplicationId;

        /// <summary>
        /// Indicates whether a token should be requested for the new user.
        /// Defaults to true.
        /// </summary>
        [JsonProperty("requestToken")]
        public bool RequestToken = true;
    }
}
