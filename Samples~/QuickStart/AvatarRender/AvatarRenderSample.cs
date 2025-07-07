
using PlayerZero.Data;
using PlayerZero.Runtime.Sdk;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerZero.Samples
{
    public class AvatarRenderSample : MonoBehaviour
    {
        [SerializeField] private string avatarId;
        [SerializeField] private Image targetImage;
        [SerializeField] private AvatarImageConfig config;

        private async void Start()
        {
            if (targetImage == null)
                return;

            var sprite = await PlayerZeroSdk.GetIconAsync(avatarId, config);
            targetImage.sprite = sprite;
        }
    }
}
