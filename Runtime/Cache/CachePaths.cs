using UnityEngine;

namespace PlayerZero
{
    /// <summary>
    /// Provides static paths for local cache storage, including asset and icon directories,
    /// using Unity's persistent data location.
    /// </summary>
    public class CachePaths
    {
        /// <summary>
        /// The root directory for PlayerZero local cache.
        /// </summary>
        public readonly static string CACHE_ROOT = Application.persistentDataPath + "/PlayerZero/Local Cache/";

        /// <summary>
        /// The directory for cached assets.
        /// </summary>
        public readonly static string CACHE_ASSET_ROOT = CACHE_ROOT + "Assets/";

        /// <summary>
        /// The directory for cached asset icons.
        /// </summary>
        public readonly static string CACHE_ASSET_ICON_PATH = CACHE_ASSET_ROOT + "Icons/";
    }
}
