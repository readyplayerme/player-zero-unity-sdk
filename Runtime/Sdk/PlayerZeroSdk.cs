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

    /// <summary>
    /// Provides static methods for initializing Player Zero SDK, managing avatar sessions,
    /// sending analytics events, and loading avatar assets.
    /// </summary>
    public static class PlayerZeroSdk
    {
        
        private static CharacterApi _characterApi;
        private static GameEventApi _gameEventApi;
        private static FileApi _fileApi;
        private static AvatarCodeApi _avatarCodeApi;
        private static Settings _settings;

        /// <summary>
        /// Invoked when the hot-loaded avatar ID changes.
        /// </summary>
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

        /// <summary>
        /// Initializes the Player Zero SDK and its APIs.
        /// </summary>
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

        /// <summary>
        /// Retrieves an avatar icon as a Sprite for the specified avatar ID and size.
        /// </summary>
        /// <param name="avatarId">The avatar ID.</param>
        /// <param name="size">The desired icon size.</param>
        /// <returns>A task resolving to the avatar icon Sprite.</returns>
        public static async Task<Sprite> GetIconAsync(string avatarId, RenderSizeLimitType size = RenderSizeLimitType.Size64)
        {
            return await GetIconAsync(avatarId, new AvatarRenderConfig { Size = size });
        }

        /// <summary>
        /// Retrieves an avatar icon as a Sprite for the specified avatar ID and render configuration.
        /// </summary>
        /// <param name="avatarId">The avatar ID.</param>
        /// <param name="config">The render configuration.</param>
        /// <returns>A task resolving to the avatar icon Sprite.</returns>
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

        /// <summary>
        /// Gets the currently hot-loaded avatar ID from query parameters or PlayerPrefs.
        /// </summary>
        /// <returns>The hot-loaded avatar ID.</returns>
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

        /// <summary>
        /// Starts a new event session and sends the event payload to Player Zero.
        /// </summary>
        /// <typeparam name="TEvent">The event type.</typeparam>
        /// <typeparam name="TEventProperties">The event properties type.</typeparam>
        /// <param name="eventPayload">The event payload.</param>
        /// <returns>The generated session ID.</returns>
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

        /// <summary>
        /// Sends an event payload to Player Zero.
        /// </summary>
        /// <typeparam name="TEvent">The event type.</typeparam>
        /// <typeparam name="TEventProperties">The event properties type.</typeparam>
        /// <param name="eventPayload">The event payload.</param>
        /// <returns>The session ID associated with the event.</returns>
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

        /// <summary>
        /// Retrieves avatar metadata for the specified avatar ID.
        /// </summary>
        /// <param name="avatarId">The avatar ID.</param>
        /// <returns>A task resolving to the avatar metadata.</returns>
        public static async Task<Character> GetAvatarMetadataAsync(string avatarId)
        {
            Initialize();

            var response = await _characterApi.FindByIdAsync(new CharacterFindByIdRequest()
            {
                Id = avatarId,
            });

            return response.Data;
        }

        /// <summary>
        /// Retrieves an avatar ID from a code.
        /// </summary>
        /// <param name="code">The code to look up.</param>
        /// <returns>A task resolving to the avatar ID.</returns>
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

        /// <summary>
        /// Instantiates an avatar GameObject using the specified request configuration.
        /// </summary>
        /// <param name="request">The character request configuration.</param>
        /// <returns>A task resolving to the instantiated avatar GameObject.</returns>
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
        
        /// <summary>
        /// Clears the cached avatar ID from PlayerPrefs.
        /// </summary>
        public static void ClearCachedAvatarId()
        {
            PlayerPrefs.DeleteKey(CACHED_AVATAR_ID);
        }
        
        /// <summary>
        /// Shuts down the Player Zero SDK and unsubscribes from events.
        /// </summary>
        public static void Shutdown()
        {
            if (!_isInitialized)
                return;

            Application.quitting -= Shutdown;
            DeepLinkHandler.OnDeepLinkDataReceived -= OnDeepLinkDataReceived;
            _isInitialized = false;
        }

        /// <summary>
        /// Sends the player back to Player Zero with the specified score and score type.
        /// </summary>
        /// <param name="score">The score value.</param>
        /// <param name="scoreType">The score type (default: "points").</param>
        public static void SendBackToPlayerZero(int score, string scoreType = "points")
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            var gameDurationSeconds = Time.realtimeSinceStartup - startTime;
            GameEnd(score, scoreType, gameDurationSeconds, _settings.GameId);
#endif
        }
    }
}