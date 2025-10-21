using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace PlayerZero.Api
{
    /// <summary>
    /// Abstract base class for web API requests that require authentication.
    /// Extends <see cref="WebApi"/> to add authentication handling, including automatic token refresh on unauthorized responses.
    /// </summary>
    public abstract class WebApiWithAuth : WebApi
    {
        /// <summary>
        /// The authentication strategy used to add authentication to requests and handle token refresh.
        /// </summary>
        private IAuthenticationStrategy _authenticationStrategy;

        /// <summary>
        /// Sets the authentication strategy for API requests.
        /// </summary>
        /// <param name="authenticationStrategy">The authentication strategy implementation.</param>
        public void SetAuthenticationStrategy(IAuthenticationStrategy authenticationStrategy)
        {
            _authenticationStrategy = authenticationStrategy;
        }

        /// <summary>
        /// Dispatches an API request with a typed request body, ensuring authentication is applied.
        /// Automatically retries the request if authentication is refreshed after an unauthorized response.
        /// </summary>
        protected override async Task<TResponse> Dispatch<TResponse, TRequestBody>(
            ApiRequest<TRequestBody> data, CancellationToken cancellationToken = default)
        {
            return await WithAuth(
                async (updatedData) =>
                    await base.Dispatch<TResponse, TRequestBody>(updatedData, cancellationToken),
                data, cancellationToken);
        }

        /// <summary>
        /// Dispatches an API request with a string payload, ensuring authentication is applied.
        /// Automatically retries the request if authentication is refreshed after an unauthorized response.
        /// </summary>
        protected override async Task<TResponse> Dispatch<TResponse>(
            ApiRequest<string> data, CancellationToken cancellationToken = default)
        {
            return await WithAuth(
                async (updatedData) =>
                    await base.Dispatch<TResponse>(updatedData, cancellationToken),
                data, cancellationToken);
        }

        /// <summary>
        /// Handles authentication for API requests, including adding authentication headers and retrying on unauthorized responses.
        /// </summary>
        private async Task<TResponse> WithAuth<TResponse, TRequestBody>(
            Func<ApiRequest<TRequestBody>, Task<TResponse>> dispatchRequest,
            ApiRequest<TRequestBody> apiRequest,
            CancellationToken cancellationToken = default)
            where TResponse : ApiResponse, new()
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (_authenticationStrategy == null)
            {
                return await dispatchRequest(apiRequest);
            }

            // Add authentication to the request
            await _authenticationStrategy.AddAuthToRequestAsync(apiRequest, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();

            var result = await dispatchRequest(apiRequest);

            // If request succeeds or is not Unauthorized, return the result
            if (result.IsSuccess || result.Status != (int)HttpStatusCode.Unauthorized)
            {
                return result;
            }

            // Try to refresh authentication
            var refreshSucceeded = await _authenticationStrategy.TryRefreshAsync(apiRequest, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            
            if (!refreshSucceeded)
            {
                return result;
            }

            // Retry the request with refreshed authentication
            return await dispatchRequest(apiRequest);
        }
    }
}
