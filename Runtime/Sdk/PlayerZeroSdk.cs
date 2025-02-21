using System.Threading.Tasks;
using GLTFast;
using PlayerZero.Api.V1;
using UnityEngine;

namespace PlayerZero.Runtime.Sdk
{
    public struct CharacterRequestConfig
    {
        public string AvatarId { get; set; }
        
        public string AvatarUrl { get; set; }
        
        public string BlueprintId { get; set; }
        
        public Transform Parent { get; set; }
    }
    
    public static class PlayerZeroSdk
    {
        private static CharacterApi _characterApi;

        private static void Init()
        {
            if (_characterApi == null)
                _characterApi = new CharacterApi();
        }
        
        public static async Task<Character> GetAvatarMetadataAsync(
            string avatarId
        )
        {
            Init();

            var response = await _characterApi.FindByIdAsync(new CharacterFindByIdRequest()
            {
                Id = avatarId,
            });

            return response.Data;
        }

        public static async Task<GameObject> InstantiateAvatarAsync(CharacterRequestConfig request)
        {
            if (string.IsNullOrEmpty(request.AvatarId) && string.IsNullOrEmpty(request.AvatarUrl))
                Debug.LogError("One of either AvatarId or AvatarUrl must be provided.");
            
            if (!string.IsNullOrEmpty(request.AvatarId) && !string.IsNullOrEmpty(request.AvatarUrl))
                Debug.LogError("Only one of either AvatarId or AvatarUrl must be provided.");
            
            Init();

            string url;

            if (!string.IsNullOrEmpty(request.AvatarUrl))
            {
                url = $"{request.AvatarUrl}?targetBlueprintId={request.BlueprintId}";
            } else {
                var response = await _characterApi.FindByIdAsync(new CharacterFindByIdRequest()
                {
                    Id = request.AvatarId,
                });
                
                url = $"{response.Data.ModelUrl}?targetBlueprintId={request.BlueprintId}";
            }

            var gltf = new GltfImport();
            if (!await gltf.Load(url))
            {
                Debug.LogError( $"Failed to load Player Zero Character" );
            }

            var playerZeroCharacterParent = new GameObject("PlayerZeroImportContainer");

            await gltf.InstantiateSceneAsync(playerZeroCharacterParent.transform);
            
            var playerZeroCharacter = playerZeroCharacterParent.transform.GetChild(0).gameObject;
            playerZeroCharacter.transform.parent = request.Parent;

            GameObject.Destroy(playerZeroCharacterParent);
            
            playerZeroCharacter.transform.localPosition = Vector3.zero;
            playerZeroCharacter.transform.localEulerAngles = Vector3.zero;

            return playerZeroCharacter;
        }
    }
}