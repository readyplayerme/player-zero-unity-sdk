using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.Networking;

namespace PlayerZero.Api.V3
{
    public class AvatarCodeApi : WebApi
    {
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
