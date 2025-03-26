using System;
using System.Collections.Generic;
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
        private static Dictionary<string, string> parameters = new Dictionary<string, string>();

        static DeepLinkHandler()
        {
            Application.deepLinkActivated += OnDeepLinkActivated;
        }

        private static void OnDeepLinkActivated(string url)
        {
            DeeplinkURL = url;
            Debug.Log($"Deep link activated: {url}");
            parameters.Clear();
            var isNewData = true;
            if (url.Contains(LINK_NAME))
            {
                var query = QueryStringParser.Parse(url);
                if (query.TryGetValue(AVATAR_ID_KEY, out var avatarId))
                {
                    isNewData = data.AvatarId != avatarId;
                    data.AvatarId = avatarId;
                    Debug.Log($"Avatar Id: {data.AvatarId}");
                }
                if (query.TryGetValue(USER_NAME_KEY, out var userName))
                {
                    data.UserName = userName;
                    Debug.Log($"User Name: {data.UserName}");
                }
                
                foreach (var key in query.Keys)
                {
                    parameters[key] = query[key];
                }
            }

            if (!isNewData) return; // don't invoke if data has not changed
            OnDeepLinkDataReceived.Invoke(data);
        }
        
        public static void CheckForDeepLink()
        {
#if UNITY_STANDALONE_WIN && UNITY_EDITOR
        // Read command-line args (deep link will be one of them if triggered via URI)
        string[] args = Environment.GetCommandLineArgs();

        foreach (string arg in args)
        {
            if (arg.StartsWith("playerzero"))
            {
                Debug.Log("Received deep link: " + arg);

                OnDeepLinkActivated(arg);
                break;
            }
        }
#endif
            OnDeepLinkActivated(Application.absoluteURL);
        }
    }
}