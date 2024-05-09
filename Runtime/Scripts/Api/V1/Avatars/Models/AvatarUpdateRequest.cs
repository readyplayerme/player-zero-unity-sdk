﻿using System.Collections.Generic;
using Newtonsoft.Json;

namespace ReadyPlayerMe.Api.V1
{
    public class AvatarUpdateRequest
    {
        public string AvatarId { get; set; }

        public AvatarUpdateRequestBody Payload { get; set; } = new AvatarUpdateRequestBody();
    }
    
    public class AvatarUpdateRequestBody
    {
        [JsonProperty("assets")]
        public IDictionary<string, string> Assets { get; set; }
    }
}