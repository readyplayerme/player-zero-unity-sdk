using UnityEngine;
using UnityEngine.UI;
using PlayerZero.Runtime.Sdk;
using UnityEngine.Events;

namespace PlayerZero.Samples
{
    /// <summary>
    /// Demonstrates loading and instantiating an avatar using a short code via Player Zero SDK.
    /// </summary>
    public class AvatarShortCodeSample : MonoBehaviour
    {
        /// <summary>
        /// Input field for entering the avatar short code.
        /// </summary>
        [SerializeField]
        private InputField codeInputField;

        /// <summary>
        /// Parent transform where the avatar will be instantiated. If not set, the avatar is instantiated at the root.
        /// </summary>
        [SerializeField][Tooltip("Parent transform where the avatar will be instantiated. If not set, the avatar will be instantiated at the root of the scene.")]
        private Transform avatarParent;

        /// <summary>
        /// Reference to the currently loaded avatar GameObject.
        /// </summary>
        private GameObject avatar;

        [Space(5)]
        /// <summary>
        /// Event invoked when the avatar is successfully loaded.
        /// </summary>
        public UnityEvent<GameObject> OnAvatarLoaded;

        /// <summary>
        /// Event invoked when avatar loading fails.
        /// </summary>
        public UnityEvent<string> OnAvatarLoadFailed;

        /// <summary>
        /// Loads an avatar using the code from the input field, instantiates it, and invokes events based on success or failure.
        /// </summary>
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
