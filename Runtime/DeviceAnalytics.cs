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
        private static extern IntPtr  GetDeviceData();
        
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
            string jsonData = "{}";
            IntPtr ptr = GetDeviceData();
            if (ptr != IntPtr.Zero)
            {
                jsonData = Marshal.PtrToStringUTF8(ptr);
            }

            Debug.Log($"JSON DATA : {jsonData}");
            if (string.IsNullOrEmpty(jsonData))
            {
                Debug.LogError("WebGL Device Data is NULL or EMPTY!");
                jsonData = "{}"; 
            }
            try
            {
                var settings = new JsonSerializerSettings
                {
                    MissingMemberHandling = MissingMemberHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore          
                };
                deviceInfo = JsonConvert.DeserializeObject<DeviceContext>(jsonData, settings);
            }
            catch (JsonException ex)
            {
                Debug.LogError($"Failed to parse DeviceContext JSON: {ex.Message}");
                deviceInfo = new DeviceContext(); // Fallback to avoid null issues
            }
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
            var json = JsonConvert.SerializeObject(deviceInfo, Formatting.Indented);
            Debug.Log($"Device Info: {json}");
            return deviceInfo;
        }
    }
}
