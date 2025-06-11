using System.Collections.Generic;
using UnityEngine;

namespace PlayerZero.Data
{
    [CreateAssetMenu(fileName = "Character Loader Config", menuName = "Player Zero/Character Loader Config", order = 2)]
    public class CharacterLoaderConfig  : ScriptableObject
    {
        public MeshLod MeshLOD;
        
        public TextureAtlas TextureAtlas;
        
        public TextureQuality TextureQuality;
        
        public TextureChannel[] TextureChannel =
        {
            PlayerZero.Data.TextureChannel.BaseColor,
            PlayerZero.Data.TextureChannel.Normal,
            PlayerZero.Data.TextureChannel.MetallicRoughness,
            PlayerZero.Data.TextureChannel.Emissive,
            PlayerZero.Data.TextureChannel.Occlusion
        };

        public int TextureSizeLimit = 1024;

        public List<string> MorphTargets = new List<string>
        {
            "none",
        };

        public List<string> MorphTargetsGroup = new List<string>();
        
        public bool DracoCompression;
        public bool MeshCompression;
    }
}