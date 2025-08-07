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
            PlayerZero.Data.TextureChannel.baseColor,
            PlayerZero.Data.TextureChannel.normal,
            PlayerZero.Data.TextureChannel.metallicRoughness,
            PlayerZero.Data.TextureChannel.emissive,
            PlayerZero.Data.TextureChannel.occlusion
        };

        public List<string> MorphTargets = new();

        public List<string> MorphTargetsGroup = new();
        
        public bool DracoCompression;
        
        public bool MeshCompression;

        public bool RemoveSkin;
    }
}