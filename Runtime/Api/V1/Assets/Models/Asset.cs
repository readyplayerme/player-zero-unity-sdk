﻿using System;
using Newtonsoft.Json;

namespace PlayerZero.Api.V1
{
    public class Asset
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("glbUrl")]
        public string GlbUrl { get; set; }
        
        [JsonProperty("iconUrl")]
        public string IconUrl { get; set; }
        
        [JsonProperty("type")]
        public string Type { get; set; }
        
        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }
        
        [JsonProperty("updatedAt")]
        public DateTime? UpdatedAt { get; set; }
    }
}