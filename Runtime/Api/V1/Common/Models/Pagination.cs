using Newtonsoft.Json;

namespace PlayerZero.Api.V1
{
    /// <summary>
    /// Represents pagination metadata for paginated API responses.
    /// Contains information about total documents, page limits, current page, and navigation flags.
    /// </summary>
    public class Pagination
    {
        /// <summary>
        /// The total number of documents available.
        /// </summary>
        [JsonProperty("totalDocs")]
        public int TotalDocs { get; set; }

        /// <summary>
        /// The maximum number of documents per page.
        /// </summary>
        [JsonProperty("limit")]
        public int Limit { get; set; }

        /// <summary>
        /// The total number of pages available.
        /// </summary>
        [JsonProperty("TotalPages")]
        public int TotalPages { get; set; }

        /// <summary>
        /// The current page number.
        /// </summary>
        [JsonProperty("page")]
        public int Page { get; set; }

        /// <summary>
        /// The paging counter for the current page.
        /// </summary>
        [JsonProperty("pagingCounter")]
        public int PagingCounter { get; set; }

        /// <summary>
        /// Indicates if there is a previous page.
        /// </summary>
        [JsonProperty("hasPrevPage")]
        public bool HasPrevPage { get; set; }

        /// <summary>
        /// Indicates if there is a next page.
        /// </summary>
        [JsonProperty("hasNextPage")]
        public bool HasNextPage { get; set; }

        /// <summary>
        /// The previous page number, if available.
        /// </summary>
        [JsonProperty("prevPage")]
        public int PrevPage { get; set; }

        /// <summary>
        /// The next page number, if available.
        /// </summary>
        [JsonProperty("nextPage")]
        public int NextPage { get; set; }
    }
}