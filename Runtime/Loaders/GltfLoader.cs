using System.Threading.Tasks;
using UnityEngine;

#if PZERO_UNITY_GLTF
using UnityGLTF;
#else 
using GLTFast;
#endif

namespace PlayerZero.Runtime.Sdk
{
    public static class GltfLoader
    {
        public static IGltfLoader Loader;
        
        static GltfLoader() 
        {
#if PZERO_UNITY_GLTF
            Loader = new UnityGltfLoader();
#else
            Loader = new GltFastLoader();
#endif
        }
        public static async Task<GameObject> LoadModelAsync(string url)
        {
            if (Loader != null) return await Loader.LoadModelAsync(url);
            Debug.LogError("GltfLoader is not initialized. Please ensure the appropriate loader is set.");
            return null;
        }
    }
}
