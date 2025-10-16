using System.Collections.Generic;
using UnityEngine;

namespace PlayerZero.Data
{
    /// <summary>
    /// Defines configuration options for character loading, including mesh LOD, texture settings,
    /// morph targets, and compression options.
    /// </summary>
    [CreateAssetMenu(fileName = "Character Loader Config", menuName = "Player Zero/Character Loader Config", order = 2)]
    public class CharacterLoaderConfig : ScriptableObject
    {
        /// <summary>
        /// Level of detail for the character mesh.
        /// </summary>
        public MeshLod MeshLOD;

        /// <summary>
        /// Texture atlas configuration, use this to merge meshes, materials and textures. If None is selected, no atlas will be created and meshes will not be merged.
        /// </summary>
        public TextureAtlas TextureAtlas = TextureAtlas.None;

        /// <summary>
        /// Quality setting for character textures.
        /// </summary>
        public TextureQuality TextureQuality = TextureQuality.High;

        /// <summary>
        /// Maximum allowed texture size.
        /// </summary>
        public TextureSizeLimit TextureSizeLimit = TextureSizeLimit.Size1024;

        /// <summary>
        /// Array of texture channels to use for the character.
        /// </summary>
        public TextureChannel[] TextureChannel =
        {
            PlayerZero.Data.TextureChannel.baseColor,
            PlayerZero.Data.TextureChannel.normal,
            PlayerZero.Data.TextureChannel.metallicRoughness,
            PlayerZero.Data.TextureChannel.emissive,
            PlayerZero.Data.TextureChannel.occlusion
        };

        /// <summary>
        /// List of morph target names for character customization.
        /// </summary>
        public List<string> MorphTargets = new();

        /// <summary>
        /// List of morph target group names.
        /// </summary>
        public List<string> MorphTargetsGroup = new();

        /// <summary>
        /// Enables Draco mesh compression.
        /// </summary>
        public bool DracoCompression;

        /// <summary>
        /// Enables Unity mesh compression.
        /// </summary>
        public bool MeshCompression;
    }
}