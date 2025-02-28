using System;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerZero.Runtime.DeepLinking
{
    public class DeepLinkManager : MonoBehaviour
    { 
        public static DeepLinkManager Instance { get; private set; }

        public string DeeplinkURL { get; private set; } = "[none]";
        
        public Action<DeepLinkData> OnDeepLinkDataReceived;
        
        private const string LINK_NAME = "playerzero";
        private const string AVATAR_ID_KEY = "avatarId";
        private const string USER_NAME_KEY = "userName";
        
        private DeepLinkData data;
        private Dictionary<string, string> parameters = new Dictionary<string, string>();

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                Application.deepLinkActivated += OnDeepLinkActivated;
                var url = Application.absoluteURL;
                if(!string.IsNullOrEmpty(url))
                {
                    
                    OnDeepLinkActivated(url);
                }
            }
            else
            {
                Destroy(gameObject);
            }
        }

        protected virtual void OnDeepLinkActivated(string url)
        {
            DeeplinkURL = url;
            parameters.Clear();
            Debug.Log($"Deep link activated with url: {DeeplinkURL}");
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
                    Debug.Log($"Extracted Avatar ID: {avatarId}.");
                }
                if (parameters.TryGetValue(USER_NAME_KEY, out var userName))
                {
                    data.UserName = userName;
                    Debug.Log($"Extracted user Name: {userName}.");
                }
            }
            OnDeepLinkDataReceived.Invoke(data);
        }
    }
}