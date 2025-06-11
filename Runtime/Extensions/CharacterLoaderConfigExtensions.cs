using System.Collections.Generic;
using System.Linq;
using PlayerZero.Data;
namespace PlayerZero
{
    public static class CharacterLoaderConfigExtensions
    {
        public static string GetQueryParams(this CharacterLoaderConfig config)
        {
            var queryParams = $"lod={config.MeshLOD.ToString().ToLower()}&textureAtlas={config.TextureAtlas.ToString().ToLower()}&textureQuality={config.TextureQuality.ToString().ToLower()}";

            queryParams += config.TextureSizeLimit switch
            {
                TextureSizeLimit.Size256 => "&textureSizeLimit=256",
                TextureSizeLimit.Size512 => "&textureSizeLimit=512",
                _ => "&textureSizeLimit=1024"
            };
            
            if (config.TextureChannel != null && config.TextureChannel.Length > 0)
            {
                var textureChannelvalue = $"&textureChannel={config.TextureChannel[0].ToString().ToLower()}";

                for (int i = 1; i < config.TextureChannel.Length; i++)
                {
                    var channel = config.TextureChannel[i];
                    textureChannelvalue += $",{channel.ToString().ToLower()}";
                }
                queryParams += textureChannelvalue;
            }
            
            // Ensure lists are not null
            config.MorphTargets ??= new List<string>();
            config.MorphTargetsGroup ??= new List<string>();

            var hasMorphTargets = config.MorphTargets.Count > 0;
            var hasGroups = config.MorphTargetsGroup.Count > 0;
            
            if (!hasMorphTargets && !hasGroups)
            {
                queryParams += $"&morphTargets=none";
            }
            
            if (hasMorphTargets)
            {
                var distinctMorphTargets = config.MorphTargets.Distinct();
                queryParams += $"&morphTargets={string.Join(",", distinctMorphTargets)}";
            }
            
            if (hasGroups)
            {
                var distinctGroups = config.MorphTargetsGroup.Distinct();
                queryParams += $"&morphTargetsGroup={string.Join(",", distinctGroups)}";
            }
            
            if (config.DracoCompression)
            {
                queryParams += "&dracoCompression=true";
            }
            // Draco compression is not compatible with mesh compression, so we only add mesh compression if Draco is not enabled.
            if (!config.DracoCompression && config.MeshCompression)
            {
                queryParams += "&meshCompression=true";
            }

            return queryParams;
        }
    }
}
