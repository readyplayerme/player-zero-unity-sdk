﻿using Newtonsoft.Json;
using PlayerZero.Api.V1.Contracts;

namespace PlayerZero.Api.V1
{
    public class AvatarSessionEndedEvent : IGameEventEnded<AvatarSessionEndedProperties>
    {
        [JsonProperty("event")]
        public const string Event = "avatar_session_ended";
        
        [JsonProperty("properties")]
        public AvatarSessionEndedProperties Properties { get; set; }
        
        [JsonProperty("token")]
        public string Token { get; set; }
    }
    
    public class AvatarSessionEndedProperties : IGameSession, IGame
    {
        [JsonProperty("avatar_session_id")]
        public string SessionId { get; set; }
        
        [JsonProperty("game_id")]
        public string GameId { get; set; }
    }
}