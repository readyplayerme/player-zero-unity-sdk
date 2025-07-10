using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using PlayerZero.Api.V1;
using PlayerZero.Data;
using PlayerZero.Editor.Api.V1.Analytics;
using PlayerZero.Editor.Api.V1.Analytics.Models;
using PlayerZero.Editor.Cache;
using UnityEditor;
using UnityEngine;

namespace PlayerZero.Editor.UI.ViewModels
{
    [Serializable]
    public class CharacterBlueprintViewModel
    {
        public CharacterBlueprint CharacterBlueprint { get; private set; }

        public Texture2D Image { get; private set; }
        
        private readonly AnalyticsApi analyticsApi;
        private readonly FileApi fileApi;
        
        public CharacterBlueprintViewModel(AnalyticsApi analyticsApi)
        {
            fileApi = new FileApi();
            this.analyticsApi = analyticsApi;
        }

        public async Task Init(CharacterBlueprint characterBlueprint)
        {
            CharacterBlueprint = characterBlueprint;
            Image = await fileApi.DownloadImageAsync(CharacterBlueprint.CharacterModel.IconUrl);
        }

        public async Task LoadBlueprintAsync()
        {
            var bytes = await fileApi.DownloadFileIntoMemoryAsync(CharacterBlueprint.CharacterModel.ModelUrl);
            
            var path = $"Assets/PlayerZero/Blueprints/{CharacterBlueprint.CharacterModel.Id}.glb";
            
            if (!AssetDatabase.IsValidFolder("Assets/PlayerZero"))
                AssetDatabase.CreateFolder("Assets", "PlayerZero");

            if (!AssetDatabase.IsValidFolder("Assets/PlayerZero/Blueprints"))
                AssetDatabase.CreateFolder("Assets/PlayerZero", "Blueprints");
            
#if UNITY_2020_1_OR_NEWER
            await File.WriteAllBytesAsync(path, bytes);
#else
            await Task.Run(() => File.WriteAllBytes(path, bytes));
#endif
                
            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            var character = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            var instance = PrefabUtility.InstantiatePrefab(character) as GameObject;
            
            analyticsApi.SendEvent(new AnalyticsEventRequest()
            {
                Payload = new AnalyticsEventRequestBody()
                {
                    Event = "next gen unity sdk action",
                    Properties =
                    {
                        { "type", "Import Character Blueprint" },
                        { "blueprintId", CharacterBlueprint.Id }
                    }
                }
            });
        }
    }
}