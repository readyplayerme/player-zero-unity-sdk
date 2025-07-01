using UnityEngine;

namespace PlayerZeroSDK.Runtime
{
    public static class PZeroLogger
    {
        private const string Prefix = "PZERO:";

        public static void Log(string message, Object context = null)
        {
            if (context != null)
                Debug.Log($"{Prefix} {message}", context);
            else
                Debug.Log($"{Prefix} {message}");
        }

        public static void LogWarning(string message, Object context = null)
        {
            if (context != null)
                Debug.LogWarning($"{Prefix} {message}", context);
            else
                Debug.LogWarning($"{Prefix} {message}");
        }

        public static void LogError(string message, Object context = null)
        {
            if (context != null)
                Debug.LogError($"{Prefix} {message}", context);
            else
                Debug.LogError($"{Prefix} {message}");
        }
    }
}