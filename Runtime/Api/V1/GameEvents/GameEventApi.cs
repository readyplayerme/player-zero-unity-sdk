using System.Collections.Generic;
using UnityEngine.Networking;

namespace PlayerZero.Api.V1
{
    public class GameEventApi : WebApi
    {
        private const string Resource = "public/events";
        
        /// <summary>
        /// Sends a generic game event to the analytics server asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the payload.</typeparam>
        /// <param name="request">The event data to send.</param>
        public void SendGameEvent<T>(T request)
        {
            var apiRequest = new ApiRequest<T>()
            {
                Url = $"{Settings.ApiBaseUrl}/v1/{Resource}",
                Method = UnityWebRequest.kHttpVerbPOST,
                Headers = new Dictionary<string, string>()
                {
                    { "Content-Type", "application/json" },
                },
                Payload = request
            };
            Dispatch<GameEventResponse, T>(apiRequest);
        }
    }
}