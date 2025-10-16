using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.Networking;

namespace PlayerZero.Api.V3
{
    /// <summary>
    /// Handles avatar code-related API calls, such as retrieving an avatar ID using a code.
    /// Inherits from <see cref="WebApi"/> and provides an asynchronous method for fetching avatar data.
    /// </summary>
    public class AvatarCodeApi : WebApi
    {
        /// <summary>
        /// Asynchronously retrieves the avatar ID associated with the provided code.
        /// </summary>
        /// <param name="request">The request containing the avatar code.</param>
        /// <returns>The response containing avatar code data.</returns>
        public async Task<AvatarCodeResponse> GetAvatarIdAsync(AvatarCodeRequest request)
        {
            return await Dispatch<AvatarCodeResponse>(new ApiRequest<string>
            {
                Url = $"{Settings.ApiBaseUrl}/v3/avatars/code/{request.Code}",
                Method = UnityWebRequest.kHttpVerbGET,
                Headers = new Dictionary<string, string>
                {
                    {"Content-Type", "application/json"}
                }
            });
        }
    }
}
