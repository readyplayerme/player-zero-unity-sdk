using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace PlayerZero.Api.V1
{
    public class FileApi
    {
        
        public virtual async Task<Texture2D> DownloadImageAsync(string url, CancellationToken cancellationToken)
        {
            var request = UnityWebRequestTexture.GetTexture(url);
            var op = request.SendWebRequest();
        
            while (!op.isDone)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    Debug.LogWarning($"Request cancelled: {url}");
                    request.Abort();
                    cancellationToken.ThrowIfCancellationRequested();
                }
                await Task.Yield();
            }
        
            return DownloadHandlerTexture.GetContent(request);
        }
        
        public virtual async Task<Texture2D> DownloadImageAsync(string url)
        {
            var request = UnityWebRequestTexture.GetTexture(url);
            var op = request.SendWebRequest();
        
            while (!op.isDone)
            {
                await Task.Yield();
            }
        
            return DownloadHandlerTexture.GetContent(request);
        }
        
        // TODO: Move the string path to a class of constants
        public virtual async Task<Texture2D> DownloadAssetIconAsync(Asset asset, CancellationToken cancellationToken = default)
        {
            if(!File.Exists(CachePaths.CACHE_ASSET_ICON_PATH + asset.Id))
            {
                // download thumbnail
                UnityWebRequest iconRequest = UnityWebRequest.Get(asset.IconUrl);
                iconRequest.downloadHandler = new DownloadHandlerFile(CachePaths.CACHE_ASSET_ICON_PATH + asset.Id);
                AsyncOperation iconOp = iconRequest.SendWebRequest();
                while (!iconOp.isDone)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        Debug.LogWarning($"Request cancelled: {asset.IconUrl}");
                        iconRequest.Abort();
                        cancellationToken.ThrowIfCancellationRequested();
                    }
                    await Task.Yield();
                }
            }
            
            return await GetTextureFromFile(CachePaths.CACHE_ASSET_ICON_PATH + asset.Id);
        }
        
        private async Task<Texture2D> GetTextureFromFile(string path)
        {
#if UNITY_2020_1_OR_NEWER
            byte[] bytes = await File.ReadAllBytesAsync(path);
#else
            byte[] bytes = await Task.Run(() => File.ReadAllBytes(path));
#endif
            var texture = new Texture2D(2, 2);
            texture.LoadImage(bytes);
            return texture;
        }

        public virtual async Task<Texture2D[]> DownloadImagesAsync(IEnumerable<string> urls, CancellationToken cancellationToken = default)
        {
            return await Task.WhenAll(urls.Select(DownloadImageAsync));
        }
        
        public virtual async Task<byte[]> DownloadFileIntoMemoryAsync(string url, CancellationToken cancellationToken = default)
        {
            var request = new UnityWebRequest();
            request.url = url;
            request.downloadHandler = new DownloadHandlerBuffer();

            var asyncOperation = request.SendWebRequest();

            while (!asyncOperation.isDone)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    Debug.LogWarning($"Request cancelled: {url}");
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
            {
                return request.downloadHandler.data;
            }

            Debug.LogError($"Failed to download file from url: {url} \n Error: {request.error}");
            return null;
        }

    }
}