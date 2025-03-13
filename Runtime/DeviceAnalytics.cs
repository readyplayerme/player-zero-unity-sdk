using System;
using UnityEngine;
#if UNITY_WEBGL && !UNITY_EDITOR
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
#endif

namespace PlayerZero
{
    public static class DeviceAnalytics
    {
        private const string FirstGameLoadTimeKey = "timeOfFirstGameLoad";
        
#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern string GetDeviceData();
        
        private static string GenerateDeviceId(DeviceContext deviceInfo, long firstLoadTime)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                string rawData = firstLoadTime.ToString();

                rawData += $"{deviceInfo.Os}-{deviceInfo.Browser}-{deviceInfo.GpuModel}-{deviceInfo.ScreenResolution}-{deviceInfo.CpuCores}";

                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
#endif

        public static DeviceContext GetDeviceInfo()
        {
            DeviceContext deviceInfo;
            
            var firstLoadTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            if (PlayerPrefs.HasKey(FirstGameLoadTimeKey))
            {
                // Parse stored string as long
                if (long.TryParse(PlayerPrefs.GetString(FirstGameLoadTimeKey), out long storedTime))
                {
                    firstLoadTime = storedTime;
                }
            }
            else
            {
                // Store as string to prevent int overflow
                PlayerPrefs.SetString(FirstGameLoadTimeKey, firstLoadTime.ToString());
                PlayerPrefs.Save();
            }

#if UNITY_WEBGL && !UNITY_EDITOR
            // Call JS function to get WebGL data
            var jsonData = GetDeviceData();
            deviceInfo = JsonUtility.FromJson<DeviceContext>(jsonData);
#else
            // Use Unity APIs for Mobile
            deviceInfo = new DeviceContext
            {
                DeviceId = SystemInfo.deviceUniqueIdentifier, // ✅ Uses reliable built-in UUID
                Os = SystemInfo.operatingSystem,
                GpuModel = SystemInfo.graphicsDeviceName,
                ScreenResolution = $"{Screen.width}x{Screen.height}",
                CpuCores = SystemInfo.processorCount,
                SystemMemory = SystemInfo.systemMemorySize, // RAM in MB
                DeviceModel = SystemInfo.deviceModel
            };
#endif

            // ✅ Generate device ID in Unity (WebGL only)
#if UNITY_WEBGL && !UNITY_EDITOR
            deviceInfo.DeviceId = GenerateDeviceId(deviceInfo, firstLoadTime);
#endif

            deviceInfo.TimeOfFirstGameLoad = firstLoadTime;
            return deviceInfo;
        }
    }
}
