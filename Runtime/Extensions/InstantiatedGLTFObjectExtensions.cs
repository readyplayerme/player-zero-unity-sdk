using UnityEngine;
#if PZERO_UNITY_GLTF
using UnityGLTF;
#endif

namespace PlayerZero
{
    /// <summary>
    /// Extension methods for <see cref="InstantiatedGLTFObject"/>.
    /// </summary>
    public static class InstantiatedGLTFObjectExtensions
    {
#if PZERO_UNITY_GLTF
    /// <summary>
    /// Transfers the cached GLTF data from this object to the target <see cref="GameObject"/>.
    /// Adds an <see cref="InstantiatedGLTFObject"/> component to the target if it does not exist.
    /// </summary>
    /// <param name="instantiatedGltfObject">The source GLTF object.</param>
    /// <param name="target">The target game object.</param>
    public static void TransferComponent(this InstantiatedGLTFObject instantiatedGltfObject, GameObject target)
    {
        var targetComponent = target.GetComponent<InstantiatedGLTFObject>();
        if (targetComponent == null)
        {
            targetComponent = target.AddComponent<InstantiatedGLTFObject>();
        }
        targetComponent.CachedData = instantiatedGltfObject.CachedData;
    }
#endif
    }

}