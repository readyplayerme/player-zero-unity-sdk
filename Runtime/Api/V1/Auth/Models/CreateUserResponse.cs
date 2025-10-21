using Newtonsoft.Json;

namespace PlayerZero.Api.V1
{
    /// <summary>
    /// Represents the API response for a user creation request, containing the response body with user details.
    /// Inherits common response properties from <see cref="ApiResponse"/>.
    /// </summary>
    public class CreateUserResponse : ApiResponse
    {
        /// <summary>
        /// The response body containing details of the created user.
        /// </summary>
        [JsonProperty("data")]
        public CreateUserResponseBody Data { get; set; } = new CreateUserResponseBody();
    }

    /// <summary>
    /// Contains details of the newly created user, such as ID, name, email, and token.
    /// Used for deserializing the user data from the API response.
    /// </summary>
    public class CreateUserResponseBody
    {
        /// <summary>
        /// The unique identifier of the created user.
        /// </summary>
        [JsonProperty("id")]
        public string Id;

        /// <summary>
        /// The display name of the created user.
        /// </summary>
        [JsonProperty("name")]
        public string Name;

        /// <summary>
        /// The email address of the created user.
        /// </summary>
        [JsonProperty("email")]
        public string Email;

        /// <summary>
        /// The authentication token issued for the created user.
        /// </summary>
        [JsonProperty("token")]
        public string Token;
    }
}
