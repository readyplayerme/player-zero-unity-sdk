using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using PlayerZero.Api;
using PlayerZero.Api.V1;

namespace PlayerZero
{
    /// <summary>
    /// Utility component to display a Ready Player Me avatar render.
    /// Provide an avatar id and the sprite will be loaded at runtime.
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class AvatarImageLoader : MonoBehaviour
    {
        [SerializeField] private string avatarId;
        [SerializeField] private AvatarImageParameters parameters = new AvatarImageParameters();
        [SerializeField] private bool loadOnStart = true;

        private FileApi _fileApi;

        private void Awake()
        {
            _fileApi = new FileApi();
        }

        private void Start()
        {
            if (loadOnStart)
            {
                _ = Load();
            }
        }

        /// <summary>
        /// Downloads the avatar image and assigns it to the attached Image component.
        /// </summary>
        public async Task Load()
        {
            if (string.IsNullOrEmpty(avatarId))
            {
                Debug.LogError("AvatarImageLoader requires an avatar id.");
                return;
            }
            var query = PlayerZero.Api.QueryBuilder.BuildQueryString(parameters);
            var url = $"https://avatars.readyplayer.me/{avatarId}.png{query}";
            var texture = await _fileApi.DownloadImageAsync(url);
            if (texture == null)
            {
                Debug.LogError($"Failed to download avatar image from {url}");
                return;
            }
            var sprite = Sprite.Create(
                texture,
                new Rect(0, 0, texture.width, texture.height),
                new Vector2(0.5f, 0.5f),
                100f
            );
            GetComponent<Image>().sprite = sprite;
        }
    }
}
