using System.Threading.Tasks;
using UnityEngine;
using GLTFast;

namespace PlayerZero.Runtime.Sdk
{
    public class GltFastLoader : IGltfLoader
    {
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
