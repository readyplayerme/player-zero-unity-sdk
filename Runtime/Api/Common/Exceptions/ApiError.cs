namespace PlayerZero.Api.Exceptions
{
    /// <summary>
    /// Represents an error response from the API, including a message and status code.
    /// Used for deserializing error details returned by the server.
    /// </summary>
    public class ApiError
    {
        /// <summary>
        /// The error message returned by the API.
        /// </summary>
        public string Message { get; set; }
        
        /// <summary>
        /// The HTTP status code associated with the error.
        /// </summary>
        public long Status { get; set; }
    }
}