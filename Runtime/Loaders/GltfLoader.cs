using System.Threading.Tasks;
using UnityEngine;

#if PZERO_UNITY_GLTF
using UnityGLTF;
#else
using GLTFast;
#endif

namespace PlayerZero.Runtime.Sdk
{
    /// <summary>
    /// Provides a static interface for loading GLTF models using the appropriate loader implementation
    /// based on compilation symbols. Supports both UnityGLTF and glTFast backends.
    /// </summary>
    public static class GltfLoader
    {
        /// <summary>
        /// The active GLTF loader implementation, selected at runtime.
        /// </summary>
        public static IGltfLoader Loader;

        /// <summary>
        /// Static constructor that initializes the loader implementation
        /// depending on the defined compilation symbol.
        /// </summary>
        static GltfLoader()
        {
#if PZERO_UNITY_GLTF
            Loader = new UnityGltfLoader();
#else
            Loader = new GltFastLoader();
#endif
        }

        /// <summary>
        /// Asynchronously loads a GLTF model from the specified URL using the active loader.
        /// Returns the root GameObject of the imported model, or null if loading fails.
        /// </summary>
        /// <param name="url">The URL of the GLTF model to load.</param>
        /// <returns>The root GameObject of the imported model, or null if loading fails.</returns>
        public static async Task<GameObject> LoadModelAsync(string url)
        {
            if (Loader != null) return await Loader.LoadModelAsync(url);
            Debug.LogError("GltfLoader is not initialized. Please ensure the appropriate loader is set.");
            return null;
        }
    }
}
