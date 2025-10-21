using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PlayerZero.Data;
using UnityEngine;
using UnityEngine.Networking;

namespace PlayerZero.Api
{
    /// <summary>
    /// Abstract base class for making web API requests using UnityWebRequest.
    /// Handles serialization, request dispatching, and error logging.
    /// </summary>
    public abstract class WebApi
    {
        /// <summary>
        /// Cached reference to SDK settings loaded from resources.
        /// </summary>
        private Settings _settings;
        /// <summary>
        /// Gets the SDK settings, loading them if not already cached.
        /// </summary>
        protected Settings Settings => _settings != null ? _settings : (_settings = Resources.Load<Settings>("PlayerZeroSettings"));
        
        /// <summary>
        /// Controls whether failed requests log warnings to the Unity console.
        /// </summary>
        protected bool LogWarnings = true;

        /// <summary>
        /// Dispatches an API request with a typed request body, serializing it to JSON.
        /// </summary>
        /// <typeparam name="TResponse">Type of the expected response.</typeparam>
        /// <typeparam name="TRequestBody">Type of the request body.</typeparam>
        /// <param name="data">API request data.</param>
        /// <param name="cancellationToken">Token to cancel the request.</param>
        /// <returns>Deserialized response object.</returns>
        protected virtual async Task<TResponse> Dispatch<TResponse, TRequestBody>(ApiRequest<TRequestBody> data, CancellationToken cancellationToken = default)
            where TResponse : ApiResponse, new()
        {
            var payload = JsonConvert.SerializeObject(new ApiRequestBody<TRequestBody>()
                {
                    Data = data.Payload
                }, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                }
            );

            return await Dispatch<TResponse>(new ApiRequest<string>
            {
                Headers = data.Headers,
                Method = data.Method,
                Url = data.Url,
                Payload = payload
            }, cancellationToken);
        }

        /// <summary>
        /// Dispatches an API request with a string payload, handling headers and cancellation.
        /// </summary>
        /// <typeparam name="TResponse">Type of the expected response.</typeparam>
        /// <param name="data">API request data.</param>
        /// <param name="cancellationToken">Token to cancel the request.</param>
        /// <returns>Deserialized response object.</returns>
        protected virtual async Task<TResponse> Dispatch<TResponse>(ApiRequest<string> data, CancellationToken cancellationToken = default)
            where TResponse : ApiResponse, new()
        {
            var request = new UnityWebRequest();
            request.url = data.Url;
            request.method = data.Method;
            request.downloadHandler = new DownloadHandlerBuffer();

            if (data.Headers != null)
            {
                foreach (var header in data.Headers)
                {
                    request.SetRequestHeader(header.Key, header.Value);
                }
            }

            if (!string.IsNullOrEmpty(data.Payload))
            {
                var bodyRaw = Encoding.UTF8.GetBytes(data.Payload);
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            }
            var asyncOperation = request.SendWebRequest();

            while (!asyncOperation.isDone)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    Debug.LogWarning($"Request cancelled: {data.Url}");
                    request.Abort();
                    cancellationToken.ThrowIfCancellationRequested();
                }
                await Task.Yield();
            }
#if UNITY_2020_1_OR_NEWER
            if (request.result == UnityWebRequest.Result.Success)
#else
            if (!request.isNetworkError && !request.isHttpError)
#endif
                return JsonConvert.DeserializeObject<TResponse>(request.downloadHandler.text);

            if (LogWarnings)
                Debug.LogWarning($"Request failed - {request.error} - {request.url}\n{request.downloadHandler.text}");

            return new TResponse()
            {
                IsSuccess = false,
                Status = request.responseCode,
                Error = request.error
            };
        }

        /// <summary>
        /// Internal helper class for wrapping request body data for serialization.
        /// </summary>
        private class ApiRequestBody<T>
        {
            [JsonProperty("data")] public T Data { get; set; }
        }
    }
}