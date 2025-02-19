using System.Threading.Tasks;
using GLTFast;
using PlayerZero.Api.V1;
using UnityEngine;

namespace PlayerZero.Runtime.Sdk
{
    public static class PlayerZeroSdk
    {
        private static CharacterApi _characterApi;

        public static void Init()
        {
            if (_characterApi == null)
                _characterApi = new CharacterApi();
        }

        public static async Task<GameObject> InstantiateAvatarAsync(
            string avatarId,
            string blueprintId,
            Transform parent = null
            )
        {
            Init();

            var response = await _characterApi.FindByIdAsync(new CharacterFindByIdRequest()
            {
                Id = avatarId,
            });
			
            var url = $"{response.Data.ModelUrl}?targetBlueprintId={blueprintId}";
			
            var gltf = new GltfImport();
            if (!await gltf.Load(url))
            {
                Debug.LogError( $"Failed to load Player Zero Character" );
            }

            var playerZeroCharacterParent = new GameObject("PlayerZeroImportContainer");

            await gltf.InstantiateSceneAsync(playerZeroCharacterParent.transform);
            
            var playerZeroCharacter = playerZeroCharacterParent.transform.GetChild(0).gameObject;
            playerZeroCharacter.transform.parent = parent;

            GameObject.Destroy(playerZeroCharacterParent);

            return playerZeroCharacter;
        }
    }
}