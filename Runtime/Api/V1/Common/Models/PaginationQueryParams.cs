using Newtonsoft.Json;

namespace PlayerZero.Api.V1
{
    /// <summary>
    /// Represents query parameters for paginated API requests.
    /// Allows specifying page size, page number, and sort order.
    /// </summary>
    public class PaginationQueryParams
    {
        /// <summary>
        /// The maximum number of items per page. Defaults to 10.
        /// </summary>
        [JsonProperty("limit")]
        public int Limit { get; set; } = 10;

        /// <summary>
        /// The page number to retrieve.
        /// </summary>
        [JsonProperty("page")]
        public int Page { get; set; }

        /// <summary>
        /// The sort order for the results.
        /// </summary>
        [JsonProperty("order")]
        public string Order { get; set; }
    }
}