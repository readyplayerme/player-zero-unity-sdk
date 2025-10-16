using Newtonsoft.Json;

namespace PlayerZero.Api.V1
{
    /// <summary>
    /// Represents the API response for a character retrieval by ID request.
    /// Contains the character data returned from the API.
    /// Inherits common response properties from <see cref="ApiResponse"/>.
    /// </summary>
    public class CharacterFindByIdResponse : ApiResponse
    {
        /// <summary>
        /// The character data returned by the API.
        /// </summary>
        [JsonProperty("data")]
        public Character Data { get; set; }
    }

}