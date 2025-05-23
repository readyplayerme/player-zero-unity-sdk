﻿using System.Collections.Generic;
using System.Threading.Tasks;
using PlayerZero.Api;
using PlayerZero.Editor.Api.V1.Auth.Models;
using UnityEngine.Networking;

namespace PlayerZero.Editor.Api.V1.Auth
{
    public sealed class DeveloperAuthApi : WebApi
    {
        public async Task<DeveloperLoginResponse> LoginAsync(DeveloperLoginRequest request)
        {
            return await Dispatch<DeveloperLoginResponse, DeveloperLoginRequestBody>(
                new ApiRequest<DeveloperLoginRequestBody>()
                {
                    Url = $"{Settings.ApiBaseAuthUrl}/login",
                    Method = UnityWebRequest.kHttpVerbPOST,
                    Headers = new Dictionary<string, string>()
                    {
                        { "Content-Type", "application/json" },
                    },
                    Payload = request.Payload
                }
            );
        }
    }
}