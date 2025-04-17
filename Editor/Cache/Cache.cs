using System.IO;
using UnityEngine;
using AssetDatabase = UnityEditor.AssetDatabase;

namespace PlayerZero.Editor.Cache
{
    public abstract class Cache
    {
        private const string BaseDirectory = "Assets/PlayerZero/Resources/";

        private readonly string _name;

        protected string CacheDirectory => BaseDirectory + _name;

        protected Cache(string name)
        {
            _name = name;

            EnsureFoldersExist();
        }

        private void EnsureFoldersExist()
        {
            if (!AssetDatabase.IsValidFolder("Assets/PlayerZero"))
                AssetDatabase.CreateFolder("Assets", "PlayerZero");

            if (!AssetDatabase.IsValidFolder("Assets/PlayerZero/Resources"))
                AssetDatabase.CreateFolder("Assets/PlayerZero", "Resources");

            if (AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/PlayerZero/Resources/README.txt") == null)
                CreateTextAsset("README.txt",
                    "This folder is managed by the PlayerZero SDK, and you should not make manual changes.",
                    "Assets/PlayerZero/Resources"
                );

            if (!AssetDatabase.IsValidFolder($"Assets/PlayerZero/Resources/{_name}"))
                AssetDatabase.CreateFolder("Assets/PlayerZero/Resources", _name);

            AssetDatabase.Refresh();
        }

        private static void CreateTextAsset(string name, string content, string directory)
        {
            File.WriteAllText($"{directory}/{name}", content);
        }

        public static string FindAssetGuid(Object asset)
        {
            if (asset == null)
                return string.Empty;

            var assetPath = AssetDatabase.GetAssetPath(asset);

            return string.IsNullOrEmpty(assetPath)
                ? string.Empty
                : AssetDatabase.AssetPathToGUID(assetPath);
        }
    }
}