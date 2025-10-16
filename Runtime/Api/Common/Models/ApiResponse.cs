namespace PlayerZero.Api
{
    /// <summary>
    /// Abstract base class for API responses, providing common properties for success status, HTTP status code, and error message.
    /// Used as the base for all typed API response models.
    /// </summary>
    public abstract class ApiResponse
    {
        /// <summary>
        /// Indicates whether the API request was successful.
        /// </summary>
        public bool IsSuccess { get; set; } = true;

        /// <summary>
        /// The HTTP status code returned by the API.
        /// </summary>
        public long Status { get; set; } = 200;

        /// <summary>
        /// The error message, if the request failed.
        /// </summary>
        public string Error { get; set; }
    }
}