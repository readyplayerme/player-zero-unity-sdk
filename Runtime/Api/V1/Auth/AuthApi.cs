using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine.Networking;

namespace PlayerZero.Api.V1
{
    /// <summary>
    /// Provides authentication-related API methods, including token refresh, login code requests, user login, and user creation.
    /// Inherits common web API functionality from <see cref="WebApi"/>.
    /// </summary>
    public class AuthApi : WebApi
    {
        /// <summary>
        /// Requests a new authentication and refresh token using an existing refresh token.
        /// </summary>
        /// <param name="request">The refresh token request payload.</param>
        /// <param name="cancellationToken">Optional cancellation token.</param>
        /// <returns>The API response containing new tokens.</returns>
        public virtual async Task<RefreshTokenResponse> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default)
        {
            return await Dispatch<RefreshTokenResponse, RefreshTokenRequestBody>(
                new ApiRequest<RefreshTokenRequestBody>()
                {
                    Url = $"{Settings.ApiBaseAuthUrl}/refresh",
                    Method = UnityWebRequest.kHttpVerbPOST,
                    Headers = new Dictionary<string, string>()
                    {
                        { "Content-Type", "application/json" },
                    },
                    Payload = request.Payload
                },
            cancellationToken);
        }
        
        /// <summary>
        /// Sends a login code to the specified user's email address.
        /// </summary>
        /// <param name="request">The request containing the user's email.</param>
        /// <param name="cancellationToken">Optional cancellation token.</param>
        /// <returns>The API response for the login code request.</returns>
        public virtual async Task<SendLoginCodeResponse> SendLoginCodeAsync(SendLoginCodeRequest request, CancellationToken cancellationToken = default)
        {
            var payload = JsonConvert.SerializeObject(request, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                }
            );
            
            var apiRequest = new ApiRequest<string>()
            {
                Url = $"{Settings.ApiBaseUrl}/v1/auth/request-login-code",
                Method = UnityWebRequest.kHttpVerbPOST,
                Headers = new Dictionary<string, string>()
                {
                    { "Content-Type", "application/json" },
                },
                Payload = payload
            };
            return await Dispatch<SendLoginCodeResponse>(apiRequest, cancellationToken);
        }
        
        /// <summary>
        /// Attempts to log in a user using their email and a received login code.
        /// </summary>
        /// <param name="request">The login request containing email and code.</param>
        /// <param name="cancellationToken">Optional cancellation token.</param>
        /// <returns>The API response containing authentication tokens.</returns>
        public virtual async Task<LoginWithCodeResponse> LoginWithCodeAsync(LoginWithCodeRequest request, CancellationToken cancellationToken = default)
        {
            
            var payload = JsonConvert.SerializeObject(request, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                }
            );
            
            var apiRequest = new ApiRequest<string>()
            {
                Url = $"{Settings.ApiBaseUrl}/v1/auth/login",
                Method = UnityWebRequest.kHttpVerbPOST,
                Headers = new Dictionary<string, string>()
                {
                    { "Content-Type", "application/json" },
                },
                Payload = payload
            };
            return await Dispatch<LoginWithCodeResponse>(apiRequest, cancellationToken);
        }

        /// <summary>
        /// Creates a new user account with the provided details.
        /// </summary>
        /// <param name="request">The user creation request payload.</param>
        /// <param name="cancellationToken">Optional cancellation token.</param>
        /// <returns>The API response for user creation.</returns>
        public virtual async Task<CreateUserResponse> CreateUserAsync(CreateUserRequest request, CancellationToken cancellationToken = default)
        {
            return await Dispatch<CreateUserResponse, CreateUserRequest>(
                new ApiRequest<CreateUserRequest>()
                {
                    Url = $"{Settings.ApiBaseUrl}/v1/users",
                    Method = UnityWebRequest.kHttpVerbPOST,
                    Headers = new Dictionary<string, string>()
                    {
                        { "Content-Type", "application/json" },
                    },
                    Payload = request
                }, cancellationToken
            );
        }
    }
}