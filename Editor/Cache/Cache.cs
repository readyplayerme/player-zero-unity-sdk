using System.IO;
using UnityEngine;
using AssetDatabase = UnityEditor.AssetDatabase;

namespace PlayerZero.Editor.Cache
{
    public abstract class Cache
    {
        private const string BASE_DIRECTORY = "Assets/PlayerZero/Resources/";

        private readonly string name;

        protected string CacheDirectory => $"{BASE_DIRECTORY}{name}";

        protected Cache(string name)
        {
            this.name = name;

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

            if (string.IsNullOrEmpty(name)) return;
            if (!AssetDatabase.IsValidFolder($"Assets/PlayerZero/Resources/{name}"))
            {
                AssetDatabase.CreateFolder("Assets/PlayerZero/Resources", name);
            }

            AssetDatabase.Refresh();
        }

        private static void CreateTextAsset(string name, string content, string directory)
        {
            File.WriteAllText($"{directory}/{name}", content);
        }
    }
}