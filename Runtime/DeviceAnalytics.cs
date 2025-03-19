using System;
using Newtonsoft.Json;
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
        private static extern IntPtr GetBrowser();
        
        private static string GenerateDeviceId(DeviceContext deviceInfo, long firstLoadTime)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                string rawData = firstLoadTime.ToString();

                rawData += $"{deviceInfo.Os}-{deviceInfo.Browser}-{deviceInfo.GpuModel}-{deviceInfo.CpuCores}={deviceInfo.SystemMemory}-{deviceInfo.DeviceModel}";

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
            var deviceInfo = new DeviceContext
            {
                DeviceId = SystemInfo.deviceUniqueIdentifier, // wont work for WebGL, gets overwritten later
                Os = SystemInfo.operatingSystem,
                GpuModel = SystemInfo.graphicsDeviceName,
                CpuCores = SystemInfo.processorCount,
                SystemMemory = SystemInfo.systemMemorySize, 
                DeviceModel = SystemInfo.deviceModel,
                Browser = GetBrowserInfo(),
                GameWindowResolution = $"{Screen.width}x{Screen.height}"
            };;
            var firstLoadTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            if (PlayerPrefs.HasKey(FirstGameLoadTimeKey))
            {
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

            // Generate device ID in Unity (WebGL only)
#if UNITY_WEBGL && !UNITY_EDITOR
            deviceInfo.DeviceId = GenerateDeviceId(deviceInfo, firstLoadTime);
#endif
            deviceInfo.TimeOfFirstGameLoad = firstLoadTime;
            var json = JsonConvert.SerializeObject(deviceInfo, Formatting.Indented);
            return deviceInfo;
        }

        private static string GetBrowserInfo()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            IntPtr ptr = GetBrowser();
            if (ptr != IntPtr.Zero)
            {
                return Marshal.PtrToStringUTF8(ptr);
            }
            Debug.LogError("WebGL Browser Data is NULL!");
            return "Unknown";
#else
            return ""; 
#endif
        }
    }
}
