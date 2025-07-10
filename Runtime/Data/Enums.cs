using UnityEngine;

namespace PlayerZero.Data
{
    /// <summary>
    ///     This enumeration describes the TextureAtlas setting options.
    /// </summary>
    /// <remarks>If set to <c>None</c> the avatar meshes, materials and textures will NOT be combined.</remarks>
    public enum TextureAtlas
    {
        None,
        [InspectorName("High (1024)")]
        High,
        [InspectorName("Medium (512)")]
        Medium,
        [InspectorName("Low (256)")]
        Low
    }

    public enum TextureQuality
    {
        High,
        Medium,
        Low
    }
    
    public enum TextureSizeLimit
    {
        [InspectorName("256px")]
        Size256 = 256,
        [InspectorName("512px")]
        Size512 = 512,
        [InspectorName("1024px")]
        Size1024 = 1024
    }
    
    /// <summary>
    ///     This enumeration describes the avatar mesh LOD (Level of Detail) options.
    /// </summary>
    public enum MeshLod
    {
        [InspectorName("High (LOD0)")]
        High,
        [InspectorName("Medium (LOD1)")]
        Medium,
        [InspectorName("Low (LOD2)")]
        Low
    }
    
    public enum TextureChannel
    {
        baseColor,
        normal,
        metallicRoughness,
        emissive,
        occlusion
    }
    
    public enum AvatarQuality
    {
        Low,
        Medium,
        High,
    }
    
    public enum CameraType
    {
        Portrait,
        Fullbody,
        Fit,
    }

    public enum ExpressionType
    {
        None,
        Scared,
        Happy,
        Lol,
        Rage,
        Sad,
    }
    
    public enum RenderSizeLimitType
    {
        [InspectorName("64px")]
        Size64 = 64,
        [InspectorName("128px")]
        Size128 = 128,
        [InspectorName("256px")]
        Size256 = 256,
        [InspectorName("512px")]
        Size512 = 512,
        [InspectorName("1024px")]
        Size1024 = 1024
    }
    
    public enum SceneType {
        Fullbody,
        Portrait,
        Custom,
    }
    
    public enum PoseType
    {
        Default,
        Dancing,
        Athlete,
        Defending,
        Fighting,
        Shrugging,
        Standing,
        Standing2,
        Standing3,
        Sitting
    }
}
