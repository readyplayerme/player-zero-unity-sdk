using Newtonsoft.Json;

namespace PlayerZero.Api.V1
{
    /// <summary>
    /// Represents a request to send a login code to a user's email via the API.
    /// Used for serializing email-based login code requests.
    /// </summary>
    public class SendLoginCodeRequest
    {
        /// <summary>
        /// The email address to which the login code will be sent.
        /// </summary>
        [JsonProperty("email")]
        public string Email;
    }
}
