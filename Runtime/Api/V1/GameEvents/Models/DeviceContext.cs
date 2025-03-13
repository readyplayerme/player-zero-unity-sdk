namespace PlayerZero
{
    [System.Serializable]
    public class DeviceContext
    {
        public string DeviceId { get; set; }
        public long TimeOfFirstGameLoad { get; set; }
        public string Os { get; set; }
        public string GpuModel { get; set; }
        public string ScreenResolution { get; set; }
        public int CpuCores { get; set; }
        public string Browser { get; set; } // WebGL only
        public int SystemMemory { get; set; } // Mobile only
        public string DeviceModel { get; set; } // Mobile only
    }
}