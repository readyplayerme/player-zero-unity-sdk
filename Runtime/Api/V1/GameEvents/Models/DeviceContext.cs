using Newtonsoft.Json;

namespace PlayerZero.Api.V1
{
    /// <summary>
    /// Represents contextual information about the user's device at the time of game load.
    /// Used for analytics and device identification.
    /// </summary>
    [System.Serializable]
    public class DeviceContext
    {
        /// <summary>
        /// The unique identifier for the device.
        /// </summary>
        [JsonProperty("device_id")]
        public string DeviceId { get; set; }

        /// <summary>
        /// The timestamp of the first game load on this device.
        /// </summary>
        [JsonProperty("time_of_first_game_load")]
        public long TimeOfFirstGameLoad { get; set; }

        /// <summary>
        /// The operating system of the device.
        /// </summary>
        [JsonProperty("os")]
        public string Os { get; set; }

        /// <summary>
        /// The GPU model of the device.
        /// </summary>
        [JsonProperty("gpu_model")]
        public string GpuModel { get; set; }

        /// <summary>
        /// The number of CPU cores in the device.
        /// </summary>
        [JsonProperty("cpu_cores")]
        public int CpuCores { get; set; }

        /// <summary>
        /// The browser name, if running in WebGL.
        /// </summary>
        [JsonProperty("browser")]
        public string Browser { get; set; } // WebGL only

        /// <summary>
        /// The amount of system memory, if running on mobile.
        /// </summary>
        [JsonProperty("system_memory")]
        public int SystemMemory { get; set; } // Mobile only

        /// <summary>
        /// The device model, if running on mobile.
        /// </summary>
        [JsonProperty("device_model")]
        public string DeviceModel { get; set; } // Mobile only

        /// <summary>
        /// The resolution of the game window. Not used in device ID generation.
        /// </summary>
        [JsonProperty("game_window_resolution")]
        public string GameWindowResolution { get; set; } // Not used in DeviceId generation
    }
}