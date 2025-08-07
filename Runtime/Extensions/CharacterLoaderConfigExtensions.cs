using System.Collections.Generic;
using System.Linq;
using PlayerZero.Data;
namespace PlayerZero
{
    public static class CharacterLoaderConfigExtensions
    {
        public static string GetQueryParams(this CharacterLoaderConfig config)
        {
            var queryParams = $"textureQuality={config.TextureQuality.ToString().ToLower()}";
            
            queryParams += config.MeshLOD switch
            {
                MeshLod.Medium => "&meshLOD=1",
                MeshLod.Low => "&meshLOD=2",
                _ => "&meshLOD=0"
            };
            queryParams += config.TextureAtlas switch
            {
                TextureAtlas.High => "&textureAtlas=1024",
                TextureAtlas.Medium => "&textureAtlas=512",
                TextureAtlas.Low => "&textureAtlas=256",
                _ => "&textureAtlas=none"
            };
            queryParams += config.TextureSizeLimit switch
            {
                TextureSizeLimit.Size256 => "&textureSizeLimit=256",
                TextureSizeLimit.Size512 => "&textureSizeLimit=512",
                _ => "&textureSizeLimit=1024"
            };
            
            if (config.TextureChannel != null && config.TextureChannel.Length > 0)
            {
                var textureChannelvalue = $"&textureChannels={config.TextureChannel[0].ToString()}";

                for (int i = 1; i < config.TextureChannel.Length; i++)
                {
                    var channel = config.TextureChannel[i];
                    textureChannelvalue += $",{channel.ToString()}";
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
            
            if (config.RemoveSkin)
            {
                queryParams += "&removeSkin=true";
            }

            return queryParams;
        }
    }
}
