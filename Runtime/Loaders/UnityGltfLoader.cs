using System.Threading.Tasks;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityGLTF;

#if PZERO_UNITY_GLTF
using UnityGLTF.Loader;
using UnityGLTF.Plugins;
#endif

namespace PlayerZero.Runtime.Sdk
{
    public class UnityGltfLoader : IGltfLoader
    {
        public async Task<GameObject> LoadModelAsync(string url)
        {
#if PZERO_UNITY_GLTF
            var importOpt = new ImportOptions();
            var uri = new System.Uri(url);
            var directory = uri.AbsoluteUri.Substring(0, uri.AbsoluteUri.LastIndexOf('/'));
            importOpt.DataLoader = new UnityWebRequestLoader(directory);
            var query = uri.Query;
            var filename= System.IO.Path.GetFileName(uri.AbsolutePath);
            Debug.Log($"Request to directory {directory} with filename {filename} with query {query}");
            var import = new GLTFSceneImporter($"{filename}{query}", importOpt);
            await import.LoadSceneAsync();
            return import.CreatedObject;
            // var loader = new GLTFSceneImporter(
            //     url, new FileLoader(url), null);
            //
            // var parent = new GameObject("UnityGLTFModel");
            // await loader.LoadSceneAsync(-1, true);
            // return parent;
#endif
            return null;
        }
    }
}
