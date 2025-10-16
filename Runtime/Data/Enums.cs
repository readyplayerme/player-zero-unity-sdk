using UnityEngine;

namespace PlayerZero.Data
{
    /// <summary>
    /// Specifies texture atlas options for combining avatar meshes, materials, and textures.
    /// </summary>
    /// <remarks>If set to <c>None</c>, meshes, materials, and textures are not combined.</remarks>
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

    /// <summary>
    /// Specifies the quality level for avatar textures.
    /// </summary>
    public enum TextureQuality
    {
        High,
        Medium,
        Low
    }

    /// <summary>
    /// Specifies the maximum allowed texture size for avatars.
    /// </summary>
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
    /// Specifies mesh level of detail (LOD) options for avatars.
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

    /// <summary>
    /// Specifies available texture channels for avatars.
    /// </summary>
    public enum TextureChannel
    {
        baseColor,
        normal,
        metallicRoughness,
        emissive,
        occlusion
    }

    /// <summary>
    /// Specifies overall avatar image quality.
    /// </summary>
    public enum AvatarQuality
    {
        Low,
        Medium,
        High,
    }

    /// <summary>
    /// Specifies camera framing options for avatar rendering.
    /// </summary>
    public enum CameraType
    {
        Portrait,
        Fullbody,
        Fit,
    }

    /// <summary>
    /// Specifies available avatar facial expressions.
    /// </summary>
    public enum ExpressionType
    {
        None,
        Scared,
        Happy,
        Lol,
        Rage,
        Sad,
    }

    /// <summary>
    /// Specifies size limits for avatar rendering.
    /// </summary>
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

    /// <summary>
    /// Specifies scene types for avatar rendering.
    /// </summary>
    public enum SceneType {
        Fullbody,
        Portrait,
        Custom,
    }

    /// <summary>
    /// Specifies pose options for avatar rendering.
    /// </summary>
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
