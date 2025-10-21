using System.Threading.Tasks;
using PlayerZero;
using PlayerZero.Data;
using PlayerZero.Runtime.Sdk;
using UnityEngine;

namespace PlayerZero.Samples
{
    /// <summary>
    /// Loads and instantiates a Player Zero avatar using a specified avatar ID and configuration.
    /// Transfers the loaded avatar's mesh to the current GameObject.
    /// </summary>
    public class SampleAvatarLoader : MonoBehaviour
    {
        /// <summary>
        /// The ID of the avatar to load.
        /// </summary>
        [SerializeField]
        private string avatarId = "67a1d5f31afad770c44e1542";

        /// <summary>
        /// Whether to load the avatar automatically on Start.
        /// </summary>
        [SerializeField]
        private bool loadOnStart = true;

        /// <summary>
        /// Configuration for loading the character.
        /// </summary>
        [SerializeField]
        private CharacterLoaderConfig characterLoaderConfig;

        /// <summary>
        /// Utility for transferring the loaded avatar mesh to this GameObject.
        /// </summary>
        private MeshTransfer meshTransfer = new MeshTransfer();

        /// <summary>
        /// Loads the avatar on Start if <see cref="loadOnStart"/> is true.
        /// </summary>
        private async void Start()
        {
            if (loadOnStart)
            {
                await LoadAvatar();
            }
        }

        /// <summary>
        /// Loads the avatar metadata, instantiates the avatar, and transfers its mesh to this GameObject.
        /// </summary>
        private async Task LoadAvatar()
        {
            var playerZeroCharacterParent = new GameObject($"Avatar_{avatarId}");

            var response = await PlayerZeroSdk.GetAvatarMetadataAsync(avatarId);
            var characterRequestConfig = new CharacterRequestConfig()
            {
                AvatarId = avatarId,
                BlueprintId = response.BlueprintId,
                Parent = playerZeroCharacterParent.transform,
                CharacterConfig = characterLoaderConfig
            };

            var avatar = await PlayerZeroSdk.InstantiateAvatarAsync(characterRequestConfig);
            meshTransfer.Transfer(playerZeroCharacterParent, gameObject);
        }
    }
}
