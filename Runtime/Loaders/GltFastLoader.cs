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
            playerZeroCharacter.transform.parent = null;
            Object.Destroy(playerZeroCharacterParent);
            return playerZeroCharacter;

            //return null;

        }
    }
}