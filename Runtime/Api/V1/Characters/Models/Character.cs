﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace PlayerZero.Api.V1
{
    public class Character
    {
        [JsonProperty("_id")]
        public string Id { get; set; }
        
        [JsonProperty("blueprintId")]
        public string BlueprintId { get; set; }
        
        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("modelUrl")]
        public string ModelUrl { get; set; }
        
        [JsonProperty("iconUrl")]
        public string IconUrl { get; set; }
        
        [JsonProperty("assets")]
        public IDictionary<string, string[]> Assets { get; set; }
        
        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }
        
        [JsonProperty("updatedAt")]
        public DateTime? UpdatedAt { get; set; }
    }
}