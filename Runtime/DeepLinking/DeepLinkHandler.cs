using System;
using UnityEngine;


namespace PlayerZero.Runtime.DeepLinking
{
    public static class DeepLinkHandler 
    { 
        public static string DeeplinkURL { get; private set; } = "[none]";
        
        public static Action<DeepLinkData> OnDeepLinkDataReceived;
        
        private const string LINK_NAME = "playerzero";
        private const string AVATAR_ID_KEY = "avatarId";
        private  const string USER_NAME_KEY = "userName";
        
        private static DeepLinkData data;

        static DeepLinkHandler()
        {
            Application.deepLinkActivated += OnDeepLinkActivated;
        }

        private static void OnDeepLinkActivated(string url)
        {
            DeeplinkURL = url;
            if (url.Contains(LINK_NAME))
            {
                var parameters = ZeroQueryParams.GetParams();
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
#if UNITY_STANDALONE_WIN && UNITY_EDITOR
        // Read command-line args (deep link will be one of them if triggered via URI)
        var args = Environment.GetCommandLineArgs();

        foreach (var arg in args)
        {
            if (arg.StartsWith(LINK_NAME))
            {
                Debug.Log($"Received deep link: {arg}");

                OnDeepLinkActivated(arg);
                break;
            }
        }

        return;
#endif
            OnDeepLinkActivated(Application.absoluteURL);
        }
    }
}