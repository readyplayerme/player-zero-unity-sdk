using System;

namespace PlayerZero.Api.Exceptions
{
    /// <summary>
    /// Exception type representing an error returned from the API.
    /// Wraps an <see cref="ApiError"/> object containing error details.
    /// </summary>
    public class ApiException : Exception
    {
        /// <summary>
        /// The error details returned by the API.
        /// </summary>
        public ApiError Error { get; set; }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiException"/> class with the specified API error.
        /// </summary>
        /// <param name="error">The API error details.</param>
        public ApiException(ApiError error)
        {
            Error = error;
        }
    }
}