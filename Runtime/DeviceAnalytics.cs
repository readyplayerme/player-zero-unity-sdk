using System.Runtime.InteropServices;
using UnityEngine;

namespace PlayerZero
{

    public static class DeviceAnalytics
    {
#if UNITY_WEBGL
        [DllImport("__Internal")]
        private static extern string GetDeviceData();
#endif

        public static DeviceContext GetDeviceInfo()
        {
            DeviceContext deviceInfo;

#if UNITY_WEBGL
            // Call JS function to get WebGL data
            var jsonData = GetDeviceData();
            deviceInfo = JsonUtility.FromJson<DeviceContext>(jsonData);
#else
        // Use Unity APIs for mobile
        deviceInfo = new DeviceData
        {
            DeviceId = SystemInfo.deviceUniqueIdentifier,
            TimeOfFirstGameLoad = PlayerPrefs.GetString("timeOfFirstGameLoad", System.DateTime.UtcNow.ToString()),
            Os = SystemInfo.operatingSystem,
            GpuModel = SystemInfo.graphicsDeviceName,
            ScreenResolution = $"{Screen.width}x{Screen.height}",
            CpuCores = SystemInfo.processorCount,
            SystemMemory = SystemInfo.systemMemorySize, // RAM in MB
            DeviceModel = SystemInfo.deviceModel
        };

        // Store first load time if not set
        if (!PlayerPrefs.HasKey("timeOfFirstGameLoad"))
        {
            PlayerPrefs.SetString("timeOfFirstGameLoad", deviceInfo.timeOfFirstGameLoad);
            PlayerPrefs.Save();
        }
#endif
            return deviceInfo;
        }
    }
}