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

        public static void OnDeepLinkActivated(string url)
        {
            DeeplinkURL = url;
            Debug.Log($"Deep link activated: {url}");
            parameters.Clear();
            if (url.Contains(LINK_NAME))
            {
                var query = QueryStringParser.Parse(url);
                foreach (var key in query.Keys)
                {
                    parameters[key] = query[key];
                }
                
                if (parameters.TryGetValue(AVATAR_ID_KEY, out var avatarId))
                {
                    data.AvatarId = avatarId;
                    Debug.Log($"Avatar Id: {data.AvatarId}");
                }
                if (parameters.TryGetValue(USER_NAME_KEY, out var userName))
                {
                    data.UserName = userName;
                    Debug.Log($"User Name: {data.UserName}");
                }
            }
            OnDeepLinkDataReceived.Invoke(data);
        }
    }
}