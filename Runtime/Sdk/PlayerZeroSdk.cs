using System;
using System.Threading.Tasks;
using PlayerZero.Api;
using PlayerZero.Api.V1;
using PlayerZero.Api.V3;
using PlayerZero.Api.V1.Contracts;
using PlayerZero.Data;
using PlayerZero.Runtime.DeepLinking;
using UnityEngine;
using Object = UnityEngine.Object;

#if UNITY_WEBGL && !UNITY_EDITOR
using System.Runtime.InteropServices;
#endif

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
        private static AvatarCodeApi _avatarCodeApi;
        private static Settings _settings;

        public static Action<string> OnHotLoadedAvatarIdChanged;

        private const string CACHED_AVATAR_ID = "PO_HotloadedAvatarId";
        
        private static bool _isInitialized;
        private static float startTime;
        
#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern void GameEnd(int score, string scoreType, float gameDurationSeconds, string gameId);
#endif
        
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
            startTime = Time.realtimeSinceStartup;
            _settings = Resources.Load<Settings>("PlayerZeroSettings");
            if(_characterApi == null)
            {
                _characterApi = new CharacterApi();
            }
            if (_gameEventApi == null)
            {
                _gameEventApi = new GameEventApi();
            }
            if (_fileApi == null)
            {
                _fileApi = new FileApi();
            }
            if (_avatarCodeApi == null)
            {
                _avatarCodeApi = new AvatarCodeApi();
            }
            DeepLinkHandler.CheckForDeepLink();
            _isInitialized = true;
        }

        public static async Task<Sprite> GetIconAsync(string avatarId, RenderSizeLimitType size = RenderSizeLimitType.Size64)
        {
            return await GetIconAsync(avatarId, new AvatarRenderConfig { Size = size });
        }

        public static async Task<Sprite> GetIconAsync(string avatarId, AvatarRenderConfig config)
        {
            Initialize();
            var iconUrl = $"https://avatars.readyplayer.me/{avatarId}.png?{config.GetParams()}";
            
            var texture = await new FileApi().DownloadImageAsync(iconUrl);

            return Sprite.Create(
                texture,
                new Rect(0, 0, texture.width, texture.height),
                new Vector2(0.5f, 0.5f),
                100f
            );
        }

        public static string GetHotLoadedAvatarId()
        {
            Initialize();
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
            
            return string.IsNullOrEmpty(avatarId) ? _settings.DefaultAvatarId : avatarId;
        }

        public static string StartEventSession<TEvent, TEventProperties>(
            TEvent eventPayload
        ) where TEvent : IGameEventStarted<TEventProperties> where TEventProperties : class, IGameSession, IGame
        {
            Initialize();

            var sessionId = Guid.NewGuid().ToString();
            eventPayload.Properties.SessionId = sessionId;
            eventPayload.Properties.GameId = _settings.GameId;
            _gameEventApi.SendGameEventAsync<TEvent, TEventProperties>(eventPayload)
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
            
            _gameEventApi.SendGameEventAsync<TEvent, TEventProperties>(eventPayload)
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

        public static async Task<string> GetAvatarIdFromCodeAsync(string code)
        {
            Initialize();

            var response = await _avatarCodeApi.GetAvatarIdAsync(new AvatarCodeRequest
            {
                Code = code
            });

            if (!response.IsSuccess || response.Data == null)
            {
                Debug.LogError($"Failed to load avatar id for code {code}");
                return null;
            }

            PlayerPrefs.SetString(CACHED_AVATAR_ID, response.Data.AvatarId);
            return response.Data.AvatarId;
        }

        public static async Task<GameObject> InstantiateAvatarAsync(CharacterRequestConfig request)
        {
            if (request.CharacterConfig == null)
                request.CharacterConfig = ScriptableObject.CreateInstance<CharacterLoaderConfig>();

            if (string.IsNullOrEmpty(request.AvatarId) && string.IsNullOrEmpty(request.AvatarUrl))
                Debug.LogError("One of either AvatarId or AvatarUrl must be provided.");

            if (!string.IsNullOrEmpty(request.AvatarId) && !string.IsNullOrEmpty(request.AvatarUrl))
                Debug.LogError("Only one of either AvatarId or AvatarUrl must be provided.");

            Initialize();

            string url;

            var query =  request.CharacterConfig.GetQueryParams();
            
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

            var playerZeroCharacter = await GltfLoader.LoadModelAsync(url);
            if (playerZeroCharacter == null)
            {
                Debug.LogError($"Failed to load Player Zero Character");
            }
            
            playerZeroCharacter.transform.parent = request.Parent;
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

        public static void SendBackToPlayerZero(int score, string scoreType = "points")
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            var gameDurationSeconds = Time.realtimeSinceStartup - startTime;
            GameEnd(score, scoreType, gameDurationSeconds, _settings.GameId);
#endif
        }
    }
}