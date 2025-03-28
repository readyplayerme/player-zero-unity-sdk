using System;
using Newtonsoft.Json;

namespace PlayerZero.Api.V1
{
    [Serializable]
    public class BlueprintListResponse : ApiResponse
    {
        [JsonProperty("data")]
        public CharacterBlueprint[] Data { get; set; }
        
        [JsonProperty("pagination")]
        public Pagination Pagination { get; set; }
    }
}
