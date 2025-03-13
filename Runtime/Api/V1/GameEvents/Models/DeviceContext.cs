namespace PlayerZero
{
    [System.Serializable]
    public class DeviceContext
    {
        public string DeviceId;
        public string TimeOfFirstGameLoad;
        public string Os;
        public string GpuModel;
        public string ScreenResolution;
        public int CpuCores;
        public string Browser; // WebGL only
        public int SystemMemory; // Mobile only
        public string DeviceModel; // Mobile only
    }
}