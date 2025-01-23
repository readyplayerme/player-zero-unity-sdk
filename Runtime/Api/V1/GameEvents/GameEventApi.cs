using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace PlayerZero.Api.V1
{
    public class GameEventApi : WebApi
    {
        private const string Resource = "public/events";
        
        // private async Task SendEventAsync(GameEventRequest json)
        // {
        //     try 
        //     {
        //         var payload = JsonConvert.SerializeObject(json);
        //         var apiRequest = new ApiRequest<string>()
        //         {
        //             Url = $"{Settings.ApiBaseUrl}/v1/{Resource}",
        //             Method = UnityWebRequest.kHttpVerbPOST,
        //             Headers = new Dictionary<string, string>()
        //             {
        //                 { "Content-Type", "application/json" },
        //             },
        //             Payload = payload
        //         };
        //         await Dispatch<GameEventsResponse>(apiRequest);
        //     }
        //     catch (Exception ex)
        //     {
        //         Debug.LogError($"Exception occurred while sending game event: {ex.Message}");
        //     }
        // }
        
        /// <summary>
        /// Sends a generic game event to the analytics server asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the payload.</typeparam>
        /// <param name="request">The event data to send.</param>
        public void SendGameEvent<T>(T request)
        {
            try
            {
                string jsonPayload = JsonConvert.SerializeObject(request);
// #pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                //SendEventAsync(new GameEventRequest<T>(request));
// #pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

                //var payload = JsonConvert.SerializeObject(json);
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
                Dispatch<GameEventsResponse, T>(apiRequest);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            } 
        }
    }
}