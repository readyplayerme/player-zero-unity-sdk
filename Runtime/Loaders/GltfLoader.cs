using System.Threading.Tasks;
using UnityEngine;
using GLTFast;
// #elif PZERO_UNITY_GLTF
// using UnityGLTF;
// #endif

namespace PlayerZero.Runtime.Sdk
{
    public static class GltfLoader
    {
        public static IGltfLoader Loader;
        
        static GltfLoader() 
        {
            Loader = new GltFastLoader();
// #elif PZERO_UNITY_GLTF
//             Loader = new UnityGltfLoader();
// #endif
        }
        public static async Task<GameObject> LoadModelAsync(string url)
        {
            if (Loader != null) return await Loader.LoadModelAsync(url);
            Debug.LogError("GltfLoader is not initialized. Please ensure the appropriate loader is set.");
            return null;
        }
    }
}
