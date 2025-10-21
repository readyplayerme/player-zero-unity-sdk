using UnityEngine;

namespace PlayerZero.Data
{
    /// <summary>
    /// Stores configuration options for PlayerZero integration, including API endpoints,
    /// authentication, game identifiers, and versioning. Provides guidance on API key usage
    /// and supports version management in the Unity Editor.
    /// </summary>
    public class Settings : ScriptableObject
    {
        /// <summary>
        /// The base URL for PlayerZero API requests.
        /// </summary>
        [SerializeField]
        private string _apiBaseUrl = "https://api.readyplayer.me";

        /// <summary>
        /// The authentication endpoint for PlayerZero API.
        /// </summary>
        public string ApiBaseAuthUrl = "https://readyplayer.me/api/auth";

        /// <summary>
        /// The application identifier.
        /// </summary>
        public string ApplicationId = "";

        /// <summary>
        /// The PlayerZero API key. 
        /// <warning>
        /// Storing this locally will expose your API key in the game build.
        /// Prefer using a backend proxy for security.
        /// </warning>
        /// </summary>
        public string ApiKey = "";

        /// <summary>
        /// Optional proxy URL for routing API requests through your own backend.
        /// </summary>
        public string ApiProxyUrl = "";

        /// <summary>
        /// The game identifier.
        /// </summary>
        public string GameId = "";

        /// <summary>
        /// The default avatar ID.
        /// </summary>
        public string DefaultAvatarId = "68304bfa19a6664919431f4e";

        /// <summary>
        /// Resolves the API base URL, using the proxy if specified.
        /// </summary>
        public string ApiBaseUrl => string.IsNullOrEmpty(ApiProxyUrl) ? _apiBaseUrl : ApiProxyUrl;

        private string version = "2.6.1";

        /// <summary>
        /// The current package version.
        /// </summary>
        public string Version => version;

    #if UNITY_EDITOR
        /// <summary>
        /// Updates the package version and saves the asset in the Unity Editor.
        /// </summary>
        /// <param name="newVersion">The new version string.</param>
        public void SetVersion(string newVersion)
        {
            if (version == newVersion) return;
            version = newVersion;

            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.SaveAssets();
            Debug.Log($"Updated package version to {version}");
        }
    #endif
    }

}
