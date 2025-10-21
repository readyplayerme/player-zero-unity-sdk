using System.Threading.Tasks;
using UnityEngine;

#if PZERO_UNITY_GLTF
using UnityGLTF;
using UnityGLTF.Loader;
#endif

namespace PlayerZero.Runtime.Sdk
{
    /// <summary>
    /// Loads GLTF models asynchronously using the UnityGLTF library and returns the root GameObject.
    /// </summary>
    public class UnityGltfLoader : IGltfLoader
    {
        /// <summary>
        /// Asynchronously loads a GLTF model from the specified URL using UnityGLTF.
        /// Instantiates the main scene and returns the imported GameObject.
        /// </summary>
        /// <param name="url">The URL of the GLTF model to load.</param>
        /// <returns>The root GameObject of the imported model, or null if loading fails or GLTF support is not enabled.</returns>
        public async Task<GameObject> LoadModelAsync(string url)
        {
#if PZERO_UNITY_GLTF
            var importOpt = new ImportOptions();
            var uri = new System.Uri(url);
            var directory = uri.AbsoluteUri.Substring(0, uri.AbsoluteUri.LastIndexOf('/'));
            importOpt.DataLoader = new UnityWebRequestLoader(directory);
            var query = uri.Query;
            var filename= System.IO.Path.GetFileName(uri.AbsolutePath);
            var import = new GLTFSceneImporter($"{filename}{query}", importOpt);
            await import.LoadSceneAsync();
            return import.CreatedObject;
#else
            return null;
#endif
        }
    }
}
