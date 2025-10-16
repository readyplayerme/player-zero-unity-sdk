using System.Threading.Tasks;
using UnityEngine;
using GLTFast;

namespace PlayerZero.Runtime.Sdk
{
    /// <summary>
    /// Loads GLTF models asynchronously using the glTFast library and returns the root GameObject.
    /// </summary>
    public class GltFastLoader : IGltfLoader
    {
        /// <summary>
        /// Asynchronously loads a GLTF model from the specified URL.
        /// Instantiates the main scene and returns the imported GameObject.
        /// </summary>
        /// <param name="url">The URL of the GLTF model to load.</param>
        /// <returns>The root GameObject of the imported model, or null if loading fails.</returns>
        public async Task<GameObject> LoadModelAsync(string url)
        {
            var gltfImport = new GltfImport();
            var success = await gltfImport.Load(url);

            if (!success)
            {
                Debug.LogError($"glTFast failed to load: {url}");
                return null;
            }

            var playerZeroCharacterParent = new GameObject("PlayerZeroImportContainer");
            await gltfImport.InstantiateMainSceneAsync(playerZeroCharacterParent.transform);
            var playerZeroCharacter = playerZeroCharacterParent.transform.GetChild(0).gameObject;
            if (!playerZeroCharacter)
            {
                Debug.LogError("No child found in PlayerZeroImportContainer. Import may have failed.");
                Object.Destroy(playerZeroCharacterParent);
                return null;
            }
            playerZeroCharacter.transform.parent = null;
            Object.Destroy(playerZeroCharacterParent);
            return playerZeroCharacter;
        }
    }
}
