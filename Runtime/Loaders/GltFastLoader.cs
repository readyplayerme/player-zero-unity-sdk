using System.Threading.Tasks;
using PlayerZero.Runtime.Sdk;
using UnityEngine;
// #if PZERO_GLTFAST
using GLTFast;
//#endif

public class GltFastLoader : IGltfLoader
{
    public async Task<GameObject> LoadModelAsync(string url)
    {
//#if PZERO_GLTFAST
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
// #else
//         return null;
// #endif
    }
}
