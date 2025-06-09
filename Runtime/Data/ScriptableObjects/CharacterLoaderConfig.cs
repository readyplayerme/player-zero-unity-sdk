using System.Collections.Generic;
using UnityEngine;

namespace PlayerZero.Data
{
    [CreateAssetMenu(fileName = "Character Loader Config", menuName = "Player Zero/Character Loader Config", order = 2)]
    public class CharacterLoaderConfig  : ScriptableObject
    {
        [Tooltip("The mesh level of detail.")]
        public MeshLod MeshLod;
        
        [Tooltip("If set to NONE the mesh, materials and textures will not be combined into 1. (or 2 if an assets texture contains transparency)")]
        public TextureAtlas TextureAtlas;
        
        public TextureQuality TextureQuality;
        
        [Tooltip("Add textures which avatar will include")]
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
        
        public bool UseDracoCompression = true;
        public bool UseMeshOptCompression = true;
    }
}