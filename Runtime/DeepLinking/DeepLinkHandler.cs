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
            parameters.Clear();
            if (url.Contains(LINK_NAME))
            {
                var uri = new Uri(url);
                var query = System.Web.HttpUtility.ParseQueryString(uri.Query);
                foreach (var key in query.AllKeys)
                {
                    parameters[key] = query[key];
                }
                
                if (parameters.TryGetValue(AVATAR_ID_KEY, out var avatarId))
                {
                    data.AvatarId = avatarId;
                }
                if (parameters.TryGetValue(USER_NAME_KEY, out var userName))
                {
                    data.UserName = userName;
                }
            }
            OnDeepLinkDataReceived.Invoke(data);
        }
    }
}