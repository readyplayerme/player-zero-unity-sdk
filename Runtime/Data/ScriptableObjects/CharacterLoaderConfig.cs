using System.Collections.Generic;
using UnityEngine;

namespace PlayerZero.Data
{
    [CreateAssetMenu(fileName = "Character Loader Config", menuName = "Player Zero/Character Loader Config", order = 2)]
    public class CharacterLoaderConfig  : ScriptableObject
    {
        public MeshLod MeshLOD;
        
        public TextureAtlas TextureAtlas = TextureAtlas.None;
        
        public TextureQuality TextureQuality = TextureQuality.High;
        
        public TextureSizeLimit TextureSizeLimit = TextureSizeLimit.Size1024;
        
        public TextureChannel[] TextureChannel =
        {
            PlayerZero.Data.TextureChannel.BaseColor,
            PlayerZero.Data.TextureChannel.Normal,
            PlayerZero.Data.TextureChannel.MetallicRoughness,
            PlayerZero.Data.TextureChannel.Emissive,
            PlayerZero.Data.TextureChannel.Occlusion
        };

        public List<string> MorphTargets = new();

        public List<string> MorphTargetsGroup = new();
        
        public bool DracoCompression;
        
        public bool MeshCompression;
    }
}