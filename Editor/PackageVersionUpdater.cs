#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.IO;
using PlayerZero.Data;
using UnityEditor.PackageManager;

namespace PlayerZero.Editor
{

    [InitializeOnLoad]
    public static class PackageVersionUpdater
    {
        private const string PackageName = "com.playerzero.sdk";
        private const string SettingsResourcePath = "PlayerZeroSettings";

        static PackageVersionUpdater()
        {
            // Run check when Unity starts
            EditorApplication.delayCall += EnsureVersionIsUpdated;
            
            // Listen for package changes
            Events.registeredPackages += OnPackagesChanged;
        }
        
        private static void OnPackagesChanged(PackageRegistrationEventArgs args)
        {
            if (!PackageUpdated(args)) return;
            Debug.Log($"Detected update to {PackageName}. Syncing version number...");
            EnsureVersionIsUpdated();
        }

        private static bool PackageUpdated(PackageRegistrationEventArgs args)
        {
            var packageUpdated = false;
        
            // Check if our package was updated
            foreach (var package in args.added)
            {
                if (package.name.Contains(PackageName))
                {
                    packageUpdated = true;
                    break;
                }
            }
        
            foreach (var package in args.changedTo)
            {
                if (package.name.Contains(PackageName))
                {
                    packageUpdated = true;
                    break;
                }
            }

            return packageUpdated;
        }

        private static void EnsureVersionIsUpdated()
        {
            // Load package.json
            var version = GetPackageVersionFromJson();
            if (string.IsNullOrEmpty(version)) return;

            var settings = Resources.Load<Settings>(SettingsResourcePath);

            if (settings == null)
            {
                Debug.LogError("PackageSettings not found in Resources! Ensure it exists.");
                return;
            }
            
            settings.SetVersion(version);

            EditorApplication.delayCall -= EnsureVersionIsUpdated;
        }

        private static string GetPackageVersionFromJson()
        {
            var packageJson = $"Packages/{PackageName}/package.json";
            if (!File.Exists(packageJson)) return null;

            var jsonText = File.ReadAllText(packageJson);
            var packageData = JsonUtility.FromJson<PackageJson>(jsonText);
            return packageData.version;
        }

        private class PackageJson
        {
            public string version;
        }
    }
}
#endif