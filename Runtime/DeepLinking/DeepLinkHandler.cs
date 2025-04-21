using System;
using UnityEngine;


namespace PlayerZero.Runtime.DeepLinking
{
    public static class DeepLinkHandler 
    { 
        public static string DeeplinkURL { get; private set; } = "[none]";
        
        public static Action<DeepLinkData> OnDeepLinkDataReceived;
        
        private const string PZERO_LINK_NAME = "playerzero";
        private const string EPIC_LINK_NAME = "com.epicgames.launcher://";
        private const string STEAM_LINK_NAME = "steam://";
        private const string AVATAR_ID_KEY = "avatarId";
        private  const string USER_NAME_KEY = "userName";
        
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
        
        public static void CheckForDeepLink()
        {
#if UNITY_STANDALONE_WIN && !UNITY_EDITOR
            OnDeepLinkActivated(GetDesktopUrl());
            return;
#endif
            OnDeepLinkActivated(Application.absoluteURL);
        }

        public static string GetDesktopUrl()
        {
#if UNITY_STANDALONE_WIN && UNITY_EDITOR
        // Read command-line args (deep link will be one of them if triggered via URI)
        var args = Environment.GetCommandLineArgs();
         
                 foreach (var arg in args)
                 {
                     if (arg.StartsWith(PZERO_LINK_NAME, StringComparison.OrdinalIgnoreCase) || arg.StartsWith(EPIC_LINK_NAME, StringComparison.OrdinalIgnoreCase) || arg.StartsWith(STEAM_LINK_NAME, StringComparison.OrdinalIgnoreCase))
                     {
                         return arg;
                     }
                 }
#endif
            return string.Empty;
        }
    }
}