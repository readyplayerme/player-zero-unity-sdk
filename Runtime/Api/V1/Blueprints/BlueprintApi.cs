using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine.Networking;

namespace PlayerZero.Api.V1
{
    /// <summary>
    /// Provides API methods for retrieving character blueprints.
    /// Inherits common web API functionality from <see cref="WebApi"/>.
    /// </summary>
    public class BlueprintApi : WebApi
    {
        /// <summary>
        /// Retrieves a paginated list of character blueprints for a specified application.
        /// Supports filtering by archived status.
        /// </summary>
        /// <param name="request">The blueprint list request payload, including application ID and archive filter.</param>
        /// <param name="cancellationToken">Optional cancellation token.</param>
        /// <returns>The API response containing the list of character blueprints and pagination info.</returns>
        public virtual async Task<BlueprintListResponse> ListAsync(BlueprintListRequest request, CancellationToken cancellationToken = default)
        {
            var apiRequest = new ApiRequest<string>()
            {
                Url = $"{Settings.ApiBaseUrl}/v1/public/blueprints?applicationId={request.ApplicationId}&archived={request.Archived.ToString().ToLower()}",
                Method = UnityWebRequest.kHttpVerbGET,
                Headers = new Dictionary<string, string>()
                {
                    { "Content-Type", "application/json" },
                }
            };
            return await Dispatch<BlueprintListResponse>(apiRequest, cancellationToken);
        }
    }
}