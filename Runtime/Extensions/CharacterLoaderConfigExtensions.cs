using System.Collections.Generic;
using System.Linq;
using PlayerZero.Data;
namespace PlayerZero
{
    /// <summary>
    /// Extension methods for <see cref="CharacterLoaderConfig"/>.
    /// </summary>
    public static class CharacterLoaderConfigExtensions
    {
        /// <summary>
        /// Generates a query string representing the avatar loading configuration.
        /// Includes texture, mesh, morph targets, and compression options.
        /// </summary>
        /// <param name="config">The character loader configuration.</param>
        /// <returns>A query string for avatar loading.</returns>
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
            if (!config.DracoCompression && config.MeshCompression)
            {
                queryParams += "&meshCompression=true";
            }

            return queryParams;
        }
    }
}
