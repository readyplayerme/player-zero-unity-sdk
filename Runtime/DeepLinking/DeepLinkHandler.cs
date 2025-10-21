using System;
using UnityEngine;


namespace PlayerZero.Runtime.DeepLinking
{
    /// <summary>
    /// Handles deep link activation in Unity, extracting avatar and user identifiers from URLs.
    /// Notifies listeners via <see cref="OnDeepLinkDataReceived"/> when new data is received.
    /// </summary>
    public static class DeepLinkHandler
    {
        /// <summary>
        /// The most recently processed deep link URL.
        /// </summary>
        public static string DeeplinkURL { get; private set; } = "[none]";

        /// <summary>
        /// Event invoked when new deep link data is received.
        /// </summary>
        public static Action<DeepLinkData> OnDeepLinkDataReceived;

        private const string PZERO_LINK_NAME = "playerzero";
        private const string EPIC_LINK_NAME = "com.epicgames.launcher://";
        private const string STEAM_LINK_NAME = "steam://";
        private const string AVATAR_ID_KEY = "avatarId";
        private const string USER_NAME_KEY = "userName";

        /// <summary>
        /// Stores the current deep link data (avatar and user identifiers) extracted from the processed URL.
        /// Updated when a new deep link is activated and passed to listeners via <see cref="OnDeepLinkDataReceived"/>.
        /// </summary>
        private static DeepLinkData data;

        static DeepLinkHandler()
        {
            Application.deepLinkActivated += OnDeepLinkActivated;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnAppStart()
        {
            Application.quitting += Shutdown;
        }

        private static void Shutdown()
        {
            Application.deepLinkActivated -= OnDeepLinkActivated;
            Application.quitting -= Shutdown;
        }

        private static void OnDeepLinkActivated(string url)
        {
            if (DeeplinkURL == url)
            {
                Debug.LogWarning($"Deeplink URL already processed: {url}");
                return;
            }
            DeeplinkURL = url;

            if (url.Contains(PZERO_LINK_NAME))
            {
                var parameters = ZeroQueryParams.GetParams(url);
                if (parameters.TryGetValue(AVATAR_ID_KEY, out var avatarId))
                {
                    data.AvatarId = avatarId;
                    Debug.Log($"DeepLink Avatar Id: {data.AvatarId}");
                }
                if (parameters.TryGetValue(USER_NAME_KEY, out var userName))
                {
                    data.UserName = userName;
                    Debug.Log($"DeepLink User Name: {data.UserName}");
                }

                OnDeepLinkDataReceived.Invoke(data);
                return;
            }
            Debug.LogWarning($"No Deeplink data found at URL: {url}");
        }

        /// <summary>
        /// Checks for a deep link on application start and processes it if found.
        /// </summary>
        public static void CheckForDeepLink()
        {
#if UNITY_STANDALONE_WIN && !UNITY_EDITOR
        OnDeepLinkActivated(GetDesktopUrl());
        return;
#endif
            OnDeepLinkActivated(Application.absoluteURL);
        }

        /// <summary>
        /// Retrieves the deep link URL from desktop launch arguments or falls back to <see cref="Application.absoluteURL"/>.
        /// </summary>
        public static string GetDesktopUrl()
        {
#if UNITY_STANDALONE_WIN && !UNITY_EDITOR
        var args = Environment.GetCommandLineArgs();

        foreach (var arg in args)
        {
            if (arg.StartsWith(PZERO_LINK_NAME, StringComparison.OrdinalIgnoreCase) || arg.StartsWith(EPIC_LINK_NAME, StringComparison.OrdinalIgnoreCase) || arg.StartsWith(STEAM_LINK_NAME, StringComparison.OrdinalIgnoreCase))
            {
                Debug.Log($"PZero Log: Detected desktop launch ARGS = {arg}");
                if (arg.Contains(PZERO_LINK_NAME))
                {
                    return arg;
                }
                // Temp workaround to prevent OnDeepLinkActivated from skipping the URL
                return $"{arg}&{PZERO_LINK_NAME}";
            }
        }
#endif
            Debug.Log($"PZero Log: No desktop launch ARGS detected, using Application.absoluteURL {Application.absoluteURL}");
            return Application.absoluteURL;
        }
    }
}