using System.Collections.Generic;

namespace PlayerZero.Api
{
    /// <summary>
    /// Represents a generic API request, including URL, HTTP method, payload, and headers.
    /// Used to encapsulate all necessary data for making web API calls.
    /// </summary>
    /// <typeparam name="T">Type of the request payload.</typeparam>
    public class ApiRequest<T>
    {
        /// <summary>
        /// The endpoint URL for the API request.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// The HTTP method (e.g., GET, POST) for the API request.
        /// </summary>
        public string Method { get; set; }
        
        /// <summary>
        /// The payload to be sent with the request.
        /// </summary>
        public T Payload { get; set; }

        /// <summary>
        /// Additional headers to include in the API request.
        /// </summary>
        public Dictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();
    }
}