using System;
using Newtonsoft.Json;
using PlayerZero.Api.V1;
using UnityEngine;
#if UNITY_WEBGL && !UNITY_EDITOR
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
#endif

namespace PlayerZero
{
    /// <summary>
    /// Provides methods for collecting device analytics information, including OS, hardware, browser, and unique device ID.
    /// </summary>
    public static class DeviceAnalytics
    {
        /// <summary>
        /// The PlayerPrefs key for storing the time of first game load.
        /// </summary>
        private const string FirstGameLoadTimeKey = "timeOfFirstGameLoad";
        
#if UNITY_WEBGL && !UNITY_EDITOR
        /// <summary>
        /// Retrieves browser information from the WebGL environment.
        /// </summary>
        [DllImport("__Internal")]
        private static extern IntPtr GetBrowser();
        
        /// <summary>
        /// Generates a unique device ID based on device info and first load time (WebGL only).
        /// </summary>
        /// <param name="deviceInfo">The device context information.</param>
        /// <param name="firstLoadTime">The time of first game load.</param>
        /// <returns>A SHA256 hash string representing the device ID.</returns>
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

        /// <summary>
        /// Collects and returns device information, including OS, hardware, browser, and unique device ID.
        /// </summary>
        /// <returns>A <see cref="DeviceContext"/> object containing device analytics data.</returns>
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

        /// <summary>
        /// Retrieves browser information for the current environment.
        /// </summary>
        /// <returns>A string representing the browser name or an empty string if not available.</returns>
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
