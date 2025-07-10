using UnityEngine;
using UnityEngine.UI;
using PlayerZero.Runtime.Sdk;
using UnityEngine.Events;

namespace PlayerZero.Samples
{
    public class AvatarShortCodeSample : MonoBehaviour
    {
        [SerializeField]
        private InputField codeInputField;
        [SerializeField][Tooltip("Parent transform where the avatar will be instantiated. If not set, the avatar will be instantiated at the root of the scene.")]
        private Transform avatarParent;
        
        private GameObject avatar;
        [Space(5)]
        public UnityEvent<GameObject> OnAvatarLoaded;
        public UnityEvent<string> OnAvatarLoadFailed;

        public async void LoadAvatar()
        {
            var code = codeInputField.text;
            var avatarId = await PlayerZeroSdk.GetAvatarIdFromCodeAsync(code);
            if (!string.IsNullOrEmpty(avatarId))
            {
                if(avatar != null)
                {
                    Destroy(avatar);
                }
                avatar = await PlayerZeroSdk.InstantiateAvatarAsync(new CharacterRequestConfig
                {
                    AvatarId = avatarId,
                    Parent = avatarParent
                });
                if (avatar != null)
                {
                    OnAvatarLoaded?.Invoke(avatar);
                    Debug.Log($"Avatar loaded successfully with code: {code}");
                    return;
                }
            }
            Debug.LogError($"Failed to load avatar with code: {code}. Please check the code and try again.");
            OnAvatarLoadFailed?.Invoke(code);
        }
    }
}
