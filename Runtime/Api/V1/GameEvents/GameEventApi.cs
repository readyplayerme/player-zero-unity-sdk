using System.Collections.Generic;
using System.Threading.Tasks;
using PlayerZero.Api.V1.Contracts;
using PlayerZero.Runtime.DeepLinking;
using UnityEngine.Networking;

namespace PlayerZero.Api.V1
{
    public class GameEventApi : WebApi
    {
        private const string PZ_TOKEN_PARAM = "token";
        private const string Resource = "public/events";
        
        /// <summary>
        /// Sends a generic game event to the analytics server asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the payload.</typeparam>
        /// <typeparam name="TEventProperties"></typeparam>
        /// <param name="request">The event data to send.</param>
        public async Task SendGameEventAsync<T, TEventProperties>(T request)
            where T : IGameEvent<TEventProperties>
            where TEventProperties : class, IGameSession
        {
            var token = ZeroQueryParams.GetParams().TryGetValue(PZ_TOKEN_PARAM, out var tokenValue) ? tokenValue : string.Empty;
            request.Token = token;

            var apiRequest = new ApiRequest<T>
            {
                Url = $"{Settings.ApiBaseUrl}/v1/{Resource}",
                Method = UnityWebRequest.kHttpVerbPOST,
                Headers = new Dictionary<string, string>
                {
                    { "Content-Type", "application/json" },
                },
                Payload = request
            };

            await Dispatch<GameEventResponse, T>(apiRequest);
        }
    }
}