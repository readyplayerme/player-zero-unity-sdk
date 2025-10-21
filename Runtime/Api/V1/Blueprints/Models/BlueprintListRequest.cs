using Newtonsoft.Json;

namespace PlayerZero.Api.V1
{
    /// <summary>
    /// Represents a request to list blueprints for a specific application.
    /// Supports pagination and filtering by archived status.
    /// Inherits pagination query parameters from <see cref="PaginationQueryParams"/>.
    /// </summary>
    public class BlueprintListRequest : PaginationQueryParams
    {
        /// <summary>
        /// The unique identifier of the application whose blueprints are being listed.
        /// </summary>
        [JsonProperty("applicationId")]
        public string ApplicationId { get; set; }

        /// <summary>
        /// Indicates whether to include archived blueprints in the results.
        /// </summary>
        [JsonProperty("archived")]
        public bool Archived { get; set; }
    }
}