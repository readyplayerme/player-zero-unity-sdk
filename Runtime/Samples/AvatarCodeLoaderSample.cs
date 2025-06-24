using UnityEngine;
using UnityEngine.UI;
using PlayerZero.Runtime.Sdk;

public class AvatarCodeLoaderSample : MonoBehaviour
{
    public InputField codeInput;
    public Transform avatarParent;

    public async void LoadAvatar()
    {
        var code = codeInput.text;
        var avatarId = await PlayerZeroSdk.GetAvatarIdFromCodeAsync(code);
        if (!string.IsNullOrEmpty(avatarId))
        {
            await PlayerZeroSdk.InstantiateAvatarAsync(new CharacterRequestConfig
            {
                AvatarId = avatarId,
                Parent = avatarParent
            });
        }
    }
}
