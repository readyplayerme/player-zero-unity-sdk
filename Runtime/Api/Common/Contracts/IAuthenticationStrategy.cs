using System.Threading;
using System.Threading.Tasks;

namespace PlayerZero.Api
{
    /// <summary>
    /// Defines methods for adding authentication to API requests and refreshing authentication tokens.
    /// Implementations handle how authentication is applied and refreshed for outgoing requests.
    /// </summary>
    public interface IAuthenticationStrategy
    {
        /// <summary>
        /// Asynchronously adds authentication information (e.g., headers or tokens) to the given API request.
        /// </summary>
        /// <typeparam name="T">Type of the request payload.</typeparam>
        /// <param name="request">The API request to modify.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        Task AddAuthToRequestAsync<T>(ApiRequest<T> request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Attempts to refresh authentication (e.g., renew tokens) if the request is unauthorized.
        /// Returns true if the refresh succeeded.
        /// </summary>
        /// <typeparam name="T">Type of the request payload.</typeparam>
        /// <param name="request">The API request that triggered the refresh.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>True if authentication was refreshed; otherwise, false.</returns>
        Task<bool> TryRefreshAsync<T>(ApiRequest<T> request, CancellationToken cancellationToken = default);
    }
}