using System;
using Newtonsoft.Json;

namespace PlayerZero.Api.V1
{
    /// <summary>
    /// Represents the API response for a blueprint list request.
    /// Contains an array of character blueprints and pagination details.
    /// Inherits common response properties from <see cref="ApiResponse"/>.
    /// </summary>
    [Serializable]
    public class BlueprintListResponse : ApiResponse
    {
        /// <summary>
        /// The array of character blueprints returned by the API.
        /// </summary>
        [JsonProperty("data")]
        public CharacterBlueprint[] Data { get; set; }

        /// <summary>
        /// Pagination information for the blueprint list results.
        /// </summary>
        [JsonProperty("pagination")]
        public Pagination Pagination { get; set; }
    }
}
