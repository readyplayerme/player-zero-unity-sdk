using System.Threading;
using System.Threading.Tasks;
using PlayerZero.Data;
using UnityEngine;

namespace PlayerZero.Api.V1
{
    /// <summary>
    /// Implements API key-based authentication for API requests.
    /// Loads the API key from project settings and attaches it to outgoing requests.
    /// </summary>
    public class ApiKeyAuthStrategy : IAuthenticationStrategy
    {
        private readonly Settings _settings;

        /// <summary>
        /// Initializes the strategy by loading API key settings from resources.
        /// </summary>
        public ApiKeyAuthStrategy()
        {
            _settings = Resources.Load<Settings>("PlayerZeroSettings");
        }

        /// <summary>
        /// Adds the API key to the request headers for authentication.
        /// </summary>
        /// <typeparam name="T">The type of the API request payload.</typeparam>
        /// <param name="request">The API request to modify.</param>
        /// <param name="cancellationToken">Optional cancellation token.</param>
        public Task AddAuthToRequestAsync<T>(ApiRequest<T> request, CancellationToken cancellationToken = default)
        {
            request.Headers["X-API-KEY"] = _settings.ApiKey;
            return Task.CompletedTask;
        }

        /// <summary>
        /// Indicates that API key authentication does not support token refresh.
        /// </summary>
        /// <typeparam name="T">The type of the API request payload.</typeparam>
        /// <param name="request">The API request.</param>
        /// <param name="cancellationToken">Optional cancellation token.</param>
        /// <returns>Always returns false.</returns>
        public Task<bool> TryRefreshAsync<T>(ApiRequest<T> request, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(false);
        }
    }

}