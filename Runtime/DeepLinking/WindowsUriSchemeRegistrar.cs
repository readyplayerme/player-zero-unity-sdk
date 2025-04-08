using UnityEngine;
#if UNITY_STANDALONE_WIN  && !UNITY_EDITOR
using System.Runtime.InteropServices;
using PlayerZero.Data;
#endif

namespace PlayerZero.Runtime.DeepLinking
{
    public static class WindowsUriSchemeRegistrar
    {
#if UNITY_STANDALONE_WIN && !UNITY_EDITOR
    [DllImport("UriSchemeRegistrar")]
    private static extern int RegisterUriScheme(string scheme, string exePath);

    public static void TryRegisterCustomScheme(string scheme)
    {
        var processModule = System.Diagnostics.Process.GetCurrentProcess().MainModule;
        if (processModule != null)
        {
            var exePath = processModule.FileName;
            int result = RegisterUriScheme(scheme, exePath);

            if (result == 0)
            {
                Debug.Log($"URI scheme '{scheme}' registered successfully.");
            }
            Debug.LogError($"Failed to register URI scheme '{scheme}'. Error code: {result}");
        }
        else
        {
            Debug.LogError($"Failed to register URI scheme '{scheme}'.");
        }
    }
#endif

        public static void Setup()
        {
#if UNITY_STANDALONE_WIN && !UNITY_EDITOR
        var settings = Resources.Load<Settings>("PlayerZeroSettings");
        if (settings != null)
        {
            TryRegisterCustomScheme(settings.GameId);
        }
        return;
#endif
            Debug.LogWarning("RegisterUriScheme is only supported on Windows Standalone builds.");
        }

    }
}
