using PlayerZero.Data;
using PlayerZero.Runtime.Sdk;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerZero.Samples
{
    /// <summary>
    /// Demonstrates how to render an avatar icon using Player Zero SDK and display it in a UI Image.
    /// </summary>
    public class AvatarRenderSample : MonoBehaviour
    {
        /// <summary>
        /// The ID of the avatar to render.
        /// </summary>
        [SerializeField] private string avatarId;

        /// <summary>
        /// The UI Image component where the avatar icon will be displayed.
        /// </summary>
        [SerializeField] private Image targetImage;

        /// <summary>
        /// The configuration for rendering the avatar icon.
        /// </summary>
        [SerializeField] private AvatarRenderConfig config;

        /// <summary>
        /// Loads and sets the avatar icon sprite on the target image at startup.
        /// </summary>
        private async void Start()
        {
            if (targetImage == null)
                return;

            var sprite = await PlayerZeroSdk.GetIconAsync(avatarId, config);
            targetImage.sprite = sprite;
        }
    }
}
