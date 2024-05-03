﻿using System.Collections.Generic;
using Newtonsoft.Json;

namespace ReadyPlayerMe.Runtime.Api.V1.Avatars.Models
{
    public class AvatarPreviewRequest
    {
        public string AvatarId { get; set; }

        public AvatarPreviewQueryParams Params { get; set; } = new AvatarPreviewQueryParams();
    }
    
    public class AvatarPreviewQueryParams
    {
        [JsonProperty("assets")]
        public IDictionary<string, string> Assets { get; set; }
    }
}