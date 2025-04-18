using System;
using System.Threading.Tasks;
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
        
        private const string CACHED_AVATAR_ID = "PO_HotloadedAvatarId";
        
        private static bool _isInitialized;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnAppStart()
        {
            Application.quitting += Shutdown;
            DeepLinkHandler.OnDeepLinkDataReceived += OnDeepLinkDataReceived;
        }

        public static void Initialize()
        {
            if (_isInitialized)
                return;

            _settings = Resources.Load<Settings>("PlayerZeroSettings");
            _characterApi ??= new CharacterApi();
            _gameEventApi ??= new GameEventApi();
            _fileApi ??= new FileApi();
            DeepLinkHandler.CheckForDeepLink();
            _isInitialized = true;
        }

        public static async Task<Sprite> GetIconAsync(string avatarId, int size = 64)
        {
            Initialize();

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
            var queryParams = ZeroQueryParams.GetParams();
            queryParams.TryGetValue("avatarId", out var avatarId);
            if (!string.IsNullOrEmpty(avatarId))
            {
                PlayerPrefs.SetString(CACHED_AVATAR_ID, avatarId);
            }
            else
            {
                // If no avatarId is found in the URL, check PlayerPrefs
                avatarId = PlayerPrefs.GetString(CACHED_AVATAR_ID, string.Empty);
            }
            return avatarId;
        }

        public static string StartEventSession<TEvent, TEventProperties>(
            TEvent eventPayload
        ) where TEvent : IGameEventStarted<TEventProperties> where TEventProperties : class, IGameSession, IGame
        {
            Initialize();

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
            Initialize();

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

        public static async Task<Character> GetAvatarMetadataAsync(string avatarId)
        {
            Initialize();

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

            Initialize();

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
        

        private static void OnDeepLinkDataReceived(DeepLinkData data)
        {
            var avatarId = string.IsNullOrEmpty(data.AvatarId) ? 
                PlayerPrefs.GetString(CACHED_AVATAR_ID, string.Empty) 
                : data.AvatarId;
            if(string.IsNullOrEmpty(avatarId))
            {
                Debug.LogWarning("No AvatarId found in the deep link data or PlayerPrefs.");
                return;
            }

            PlayerPrefs.SetString(CACHED_AVATAR_ID, avatarId);
            OnHotLoadedAvatarIdChanged?.Invoke(avatarId);
        }
        
        public static void ClearCachedAvatarId()
        {
            PlayerPrefs.DeleteKey(CACHED_AVATAR_ID);
        }
        
        public static void Shutdown()
        {
            if (!_isInitialized)
                return;

            Application.quitting -= Shutdown;
            DeepLinkHandler.OnDeepLinkDataReceived -= OnDeepLinkDataReceived;
            _isInitialized = false;
        }
    }
}