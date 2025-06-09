using System.Threading.Tasks;
using PlayerZero;
using PlayerZero.Runtime.Sdk;
using UnityEngine;

public class SampleAvatarLoader : MonoBehaviour
{
    [SerializeField]
    private string avatarId = "67a1d5f31afad770c44e1542";
    [SerializeField]
    private bool loadOnStart = true;
    
    private MeshTransfer meshTransfer = new MeshTransfer();
    
    private async void Start()
    {
        if(loadOnStart)
        {
            await LoadAvatar();
        }
    }

    private async Task LoadAvatar()
    {
        var playerZeroCharacterParent = new GameObject($"Avatar_{avatarId}");

        var response = await PlayerZeroSdk.GetAvatarMetadataAsync(avatarId);
        var characterRequestConfig = new CharacterRequestConfig()
        {
            AvatarId = avatarId,
            BlueprintId = response.BlueprintId,
            Parent = playerZeroCharacterParent.transform
        };

        var avatar = await PlayerZeroSdk.InstantiateAvatarAsync(characterRequestConfig);
        meshTransfer.Transfer(playerZeroCharacterParent, gameObject);
    }
}
