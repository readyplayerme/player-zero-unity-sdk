using UnityEngine;
#if PZERO_UNITY_GLTF
using UnityGLTF;
#endif

public static class InstantiatedGLTFObjectExtensions
{
#if PZERO_UNITY_GLTF
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
