using PlayerZero;
using PlayerZero.Runtime.Sdk;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class AvatarRender : MonoBehaviour
{
    [SerializeField] private string avatarId;
    [SerializeField] private Image targetImage;
    [FormerlySerializedAs("parameters")] [SerializeField] private AvatarImageConfig config;

    private async void Start()
    {
        if (targetImage == null)
            return;

        var sprite = await PlayerZeroSdk.GetIconAsync(avatarId, config);
        targetImage.sprite = sprite;
    }
}

