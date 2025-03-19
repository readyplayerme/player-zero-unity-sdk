using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using GLTFast;
using PlayerZero.Api;
using PlayerZero.Api.V1;
using PlayerZero.Api.V1.Contracts;
using PlayerZero.Data;
using PlayerZero.Runtime.DeepLinking;
using UnityEngine;

namespace PlayerZero.Runtime.Sdk
{
    public struct CharacterRequestConfig
    {
        public string AvatarId { get; set; }

        public string AvatarUrl { get; set; }

        public string BlueprintId { get; set; }

        public Transform Parent { get; set; }

        public CharacterLoaderConfig CharacterConfig { get; set; }
    }

    public static class PlayerZeroSdk
    {
        private static CharacterApi _characterApi;
        private static GameEventApi _gameEventApi;
        private static FileApi _fileApi;
        private static Settings _settings;

        public static Action<string> OnHotLoadedAvatarIdChanged;


        private static void Init()
        {
            if (_settings == null)
                _settings = Resources.Load<Settings>("PlayerZeroSettings");

            if (_characterApi == null)
                _characterApi = new CharacterApi();

            if (_gameEventApi == null)
                _gameEventApi = new GameEventApi();

            if (_fileApi == null)
                _fileApi = new FileApi();
            DeepLinkHandler.OnDeepLinkDataReceived += OnDeepLinkDataReceived;
        }

        public static async Task<Sprite> GetIconAsync(string avatarId, int size = 64)
        {
            Init();

            var fileApi = new FileApi();
            var iconUrl = $"https://avatars.readyplayer.me/{avatarId}.png?size={size}";
            var texture = await fileApi.DownloadImageAsync(iconUrl);

            return Sprite.Create(
                texture,
                new Rect(0, 0, texture.width, texture.height),
                new Vector2(0.5f, 0.5f),
                100f
            );
        }

        public static string GetHotLoadedAvatarId()
        {
            var fullUrl = Application.absoluteURL;
            var queryParams = GetQueryParameters(fullUrl);
            queryParams.TryGetValue("avatarId", out var avatarId);

            return avatarId;
        }

        public static string StartEventSession<TEvent, TEventProperties>(
            TEvent eventPayload
        ) where TEvent : IGameEventStarted<TEventProperties> where TEventProperties : class, IGameSession, IGame
        {
            Init();

            var sessionId = Guid.NewGuid().ToString();
            eventPayload.Properties.SessionId = sessionId;
            eventPayload.Properties.GameId = _settings.GameId;
            _gameEventApi.SendGameEventAsync(eventPayload)
                .ContinueWith(eventResponse =>
                {
                    if (eventResponse.Status != TaskStatus.RanToCompletion)
                    {
                        Debug.LogWarning("A Player Zero event failed to send.");
                    }
                });

            return eventPayload.Properties.SessionId;
        }

        public static string SendEvent<TEvent, TEventProperties>(
            TEvent eventPayload
        ) where TEvent : IGameEvent<TEventProperties> where TEventProperties : class, IGameSession, IGame
        {
            Init();

            eventPayload.Properties.GameId = _settings.GameId;
            
            _gameEventApi.SendGameEventAsync(eventPayload)
                .ContinueWith(eventResponse =>
                {
                    if (eventResponse.Status != TaskStatus.RanToCompletion)
                    {
                        Debug.LogWarning("A Player Zero event failed to send.");
                    }
                });

            return eventPayload.Properties.SessionId;
        }

        public static async Task<Character> GetAvatarMetadataAsync(
            string avatarId
        )
        {
            Init();

            var response = await _characterApi.FindByIdAsync(new CharacterFindByIdRequest()
            {
                Id = avatarId,
            });

            return response.Data;
        }

        public static async Task<GameObject> InstantiateAvatarAsync(CharacterRequestConfig request)
        {
            if (request.CharacterConfig == null)
                request.CharacterConfig = new CharacterLoaderConfig();

            if (string.IsNullOrEmpty(request.AvatarId) && string.IsNullOrEmpty(request.AvatarUrl))
                Debug.LogError("One of either AvatarId or AvatarUrl must be provided.");

            if (!string.IsNullOrEmpty(request.AvatarId) && !string.IsNullOrEmpty(request.AvatarUrl))
                Debug.LogError("Only one of either AvatarId or AvatarUrl must be provided.");

            Init();

            string url;

            var query = QueryBuilder.BuildQueryString(request.CharacterConfig);

            if (!string.IsNullOrEmpty(request.AvatarUrl))
            {
                url = string.IsNullOrEmpty(request.BlueprintId)
                    ? $"{request.AvatarUrl}?{query}"
                    : $"{request.AvatarUrl}?{query}&targetBlueprintId={request.BlueprintId}";
            }
            else
            {
                var response = await _characterApi.FindByIdAsync(new CharacterFindByIdRequest()
                {
                    Id = request.AvatarId,
                });

                url = string.IsNullOrEmpty(request.BlueprintId)
                    ? $"{response.Data.ModelUrl}?{query}"
                    : $"{response.Data.ModelUrl}?{query}&targetBlueprintId={request.BlueprintId}";
            }

            var gltf = new GltfImport();
            if (!await gltf.Load(url))
            {
                Debug.LogError($"Failed to load Player Zero Character");
            }

            var playerZeroCharacterParent = new GameObject("PlayerZeroImportContainer");

            await gltf.InstantiateSceneAsync(playerZeroCharacterParent.transform);

            var playerZeroCharacter = playerZeroCharacterParent.transform.GetChild(0).gameObject;
            playerZeroCharacter.transform.parent = request.Parent;

            GameObject.Destroy(playerZeroCharacterParent);

            playerZeroCharacter.transform.localPosition = Vector3.zero;
            playerZeroCharacter.transform.localEulerAngles = Vector3.zero;

            return playerZeroCharacter;
        }

        private static Dictionary<string, string> GetQueryParameters(string url)
        {
            if (string.IsNullOrEmpty(url))
                return new Dictionary<string, string>();

            return HttpUtility.ParseQueryString(new Uri(url).Query)
                .AllKeys
                .Where(key => key != null)
                .ToDictionary(key => key, key => HttpUtility.ParseQueryString(new Uri(url).Query)[key]);
        }

        private static void OnDeepLinkDataReceived(DeepLinkData data)
        {
            OnHotLoadedAvatarIdChanged?.Invoke(data.AvatarId);
        }
    }
}