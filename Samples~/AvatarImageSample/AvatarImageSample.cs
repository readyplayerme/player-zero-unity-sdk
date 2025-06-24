using UnityEngine;
using UnityEngine.UI;

namespace PlayerZero.Samples
{
    public class AvatarImageSample : MonoBehaviour
    {
        [SerializeField] private string avatarId;
        [SerializeField] private Image targetImage;
        [SerializeField] private AvatarImageParameters parameters = new AvatarImageParameters();

        private async void Start()
        {
            if (targetImage == null)
                return;

            var sprite = await Runtime.Sdk.PlayerZeroSdk.GetIconAsync(avatarId, parameters);
            targetImage.sprite = sprite;
        }
    }
}
