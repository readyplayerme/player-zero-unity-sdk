using PlayerZero.Runtime.DeepLinking;

namespace PlayerZero.Runtime
{
    /// <summary>
    /// Handles extraction and storage of lobby-related query parameters from deep links,
    /// including lobby ID, host status, and host region.
    /// </summary>
    public static class LobbyQueryHandler
    {
        /// <summary>
        /// The query parameter key for the lobby ID.
        /// </summary>
        private const string LobbyIdKey = "lobbyId";
        /// <summary>
        /// The query parameter key indicating host status.
        /// </summary>
        private const string HostKey = "host";
        /// <summary>
        /// The query parameter key for the host region.
        /// </summary>
        private const string HostRegionKey = "region";

        /// <summary>
        /// Gets the lobby ID extracted from the query parameters.
        /// </summary>
        public static string LobbyId { get; private set; }
        /// <summary>
        /// Gets a value indicating whether the current user is the host.
        /// </summary>
        public static bool IsHost { get; private set; }
        /// <summary>
        /// Gets the host region extracted from the query parameters.
        /// </summary>
        public static string HostRegion { get; private set; }

        /// <summary>
        /// Static constructor that checks for lobby query parameters on initialization.
        /// </summary>
        static LobbyQueryHandler()
        {
            CheckForLobby();
        }

        /// <summary>
        /// Checks for lobby-related query parameters and updates static properties accordingly.
        /// Returns true if a lobby ID is found.
        /// </summary>
        /// <returns>True if a lobby ID is present; otherwise, false.</returns>
        public static bool CheckForLobby()
        {
            var queryParams = ZeroQueryParams.GetParams();
            if (queryParams.TryGetValue(LobbyIdKey, out var id))
            {
                LobbyId = id;
            }

            if (queryParams.TryGetValue(HostKey, out var host))
            {
                IsHost = host == "true";
            }

            if (queryParams.TryGetValue(HostRegionKey, out var region))
            {
                HostRegion = region;
            }

            return !string.IsNullOrEmpty(LobbyId);
        }
    }
}
