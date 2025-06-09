using PlayerZero.Data;
using UnityEngine;

namespace PlayerZero
{
    public static class CharacterLoaderConfigExtensions
    {
        public static string GetQueryParams(this CharacterLoaderConfig config)
        {
            var queryParams = $"lod={config.MeshLod.ToString().ToLower()}&textureAtlas={config.TextureAtlas.ToString().ToLower()}&textureQuality={config.TextureQuality.ToString().ToLower()}&textureSizeLimit={config.TextureSizeLimit}";


            if (config.TextureChannel != null && config.TextureChannel.Length > 0)
            {
                var textureChannelvalue = $"&textureChannel={config.TextureChannel[0].ToString().ToLower()}";

                for (int i = 1; i < config.TextureChannel.Length; i++)
                {
                    var channel = config.TextureChannel[i];
                    // Append the channel to the textureChannelvalue string
                    textureChannelvalue += $",{channel.ToString().ToLower()}";
                }
                queryParams += textureChannelvalue;
            }
            
            if(config.MorphTargets != null && config.MorphTargets.Count > 0)
            {
                queryParams += $"&morphTargets={string.Join(",", config.MorphTargets)}";
            }
            
            Debug.Log($"CharacterLoaderConfig.GetQueryParams: {queryParams}");

            return queryParams;
        }
    }
}
