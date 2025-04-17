using System.IO;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace PlayerZero.Editor.Cache
{
    public class GlbCache : Cache
    {
        public GlbCache(string name) : base(name) {}

        public async Task Save(byte[] bytes, string id)
        {
            var path = $"{CacheDirectory}/{id}.glb";
#if UNITY_2020_1_OR_NEWER
            await File.WriteAllBytesAsync(path, bytes);
#else
            await Task.Run(() => File.WriteAllBytes(path, bytes));
#endif
                
            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        
        public GameObject Load(string id)
        {
            return AssetDatabase.LoadAssetAtPath<GameObject>($"{CacheDirectory}/{id}.glb");
        }
    }
}