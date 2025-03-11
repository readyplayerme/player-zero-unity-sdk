using UnityEngine;

namespace PlayerZero.Data
{
    public class Settings : ScriptableObject
    {
        [SerializeField]
        private string _apiBaseUrl = "https://api.readyplayer.me";

        public string ApiBaseAuthUrl = "https://readyplayer.me/api/auth";
        
        public string ApplicationId = "";

        /// <warning>
        /// Setting this property locally means that your PlayerZero API Key will be present in your game build.
        /// It is our advice that this property should not be set, and instead you should set the ApiBaseUrl and ApiBaseAuthUrl to point
        /// to your own backend server which then makes requests to the PlayerZero API.
        /// 
        /// However, it is up to you whether having your API key appear in your build is an acceptable risk as a trade off for the
        /// convenience of not having to run your own proxy backend server.
        /// </warning>

        public string ApiKey = "";

        public string ApiProxyUrl = "";

        public string GameId = "";
        public string ApiBaseUrl => string.IsNullOrEmpty(ApiProxyUrl) ? _apiBaseUrl : ApiProxyUrl;
        
        private string version = "2.1.1";
        public string Version => version;

        public void SetVersion(string newVersion)
        {
            if (version == newVersion) return;
            version = newVersion;
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.SaveAssets();
            Debug.Log($"Updated package version to {version}");
#endif
        }
    }
}
