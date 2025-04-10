using PlayerZero.Runtime.DeepLinking;

namespace PlayerZero.Runtime
{
    public static class LobbyQueryHandler
    {
        private const string LobbyIdKey = "lobbyId";
        private const string HostKey = "host";
        private const string HostRegionKey = "region";
        
        public static string LobbyId { get; private set; }
        public static bool IsHost { get; private set; }
        public static string HostRegion { get; private set; }
        
        static LobbyQueryHandler()
        {
            CheckForLobby();
        }

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
