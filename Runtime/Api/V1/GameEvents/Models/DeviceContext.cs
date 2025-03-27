using Newtonsoft.Json;

namespace PlayerZero.Api.V1
{
    [System.Serializable]
    public class DeviceContext
    {
        [JsonProperty("device_id")]
        public string DeviceId { get; set; }
        [JsonProperty("time_of_first_game_load")]
        public long TimeOfFirstGameLoad { get; set; }
        [JsonProperty("os")]
        public string Os { get; set; }
        [JsonProperty("gpu_model")]
        public string GpuModel { get; set; }
        [JsonProperty("cpu_cores")]
        public int CpuCores { get; set; }
        [JsonProperty("browser")]
        public string Browser { get; set; } // WebGL only
        [JsonProperty("system_memory")]
        public int SystemMemory { get; set; } // Mobile only
        [JsonProperty("device_model")]
        public string DeviceModel { get; set; } // Mobile only
        [JsonProperty("game_window_resolution")]
        public string GameWindowResolution { get; set; } // Not used in DeviceId generation
    }
}