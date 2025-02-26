using System;
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

        private GlbCache _characterBlueprintCache;
        
        private readonly AnalyticsApi _analyticsApi;
        private readonly FileApi _fileApi;
        
        public CharacterBlueprintViewModel(AnalyticsApi analyticsApi)
        {
            _fileApi = new FileApi();
            _analyticsApi = analyticsApi;
        }

        public async Task Init(CharacterBlueprint characterBlueprint)
        {
            _characterBlueprintCache = new GlbCache("Character Blueprints");

            CharacterBlueprint = characterBlueprint;
            Image = await _fileApi.DownloadImageAsync(CharacterBlueprint.CharacterModel.IconUrl);
        }

        public async Task LoadBlueprintAsync()
        {
            var bytes = await _fileApi.DownloadFileIntoMemoryAsync(CharacterBlueprint.CharacterModel.ModelUrl);

            await _characterBlueprintCache.Save(bytes, CharacterBlueprint.Id);

            var character = _characterBlueprintCache.Load(CharacterBlueprint.Id);
            var instance = PrefabUtility.InstantiatePrefab(character) as GameObject;
            
            _analyticsApi.SendEvent(new AnalyticsEventRequest()
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

        public void SaveCharacterBlueprintTemplate(CharacterTemplate characterTemplate)
        {
            Debug.Log($"SaveCharacterBlueprintTemplate: {characterTemplate.Name}");
        }
    }
}