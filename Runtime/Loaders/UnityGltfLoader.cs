using System.Threading.Tasks;
using UnityEngine;

#if PZERO_UNITY_GLTF
using UnityGLTF;
using UnityGLTF.Loader;
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
            var import = new GLTFSceneImporter($"{filename}{query}", importOpt);
            await import.LoadSceneAsync();
            return import.CreatedObject;
#else
            return null;
#endif
        }
    }
}
