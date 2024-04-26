﻿using Newtonsoft.Json;

namespace ReadyPlayerMe.Runtime.Api.V1.Auth.Models
{
    public class RefreshTokenRequest
    {
        public RefreshTokenRequestBody Payload { get; set; } = new RefreshTokenRequestBody();
    }

    public class RefreshTokenRequestBody
    {
        [JsonProperty("token")]
        public string Token { get; set; }
        
        [JsonProperty("refreshToken")]
        public string RefreshToken { get; set; }
    }
}