using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlayerZero.Api.V1;
using PlayerZero.Data;
using PlayerZero.Editor.Cache;
using UnityEditor;
using UnityEngine;
using CharacterTemplateConfig = PlayerZero.Data.CharacterTemplateConfig;

namespace PlayerZero.Editor
{
    public static class CharacterTemplateConfigCreator
    {
        private const string RPM_RESOURCES_PATH = "Assets/PlayerZero/Resources";

        public static async Task LoadAndCreateTemplateList(string applicationId)
        {
            ValidateFolders();
            if (string.IsNullOrEmpty(applicationId)) return;
            var templateListObject = Resources.Load<CharacterTemplateConfig>(applicationId);
            if (templateListObject == null) {
                templateListObject = ScriptableObject.CreateInstance<CharacterTemplateConfig>();
                AssetDatabase.CreateAsset(templateListObject, $"{RPM_RESOURCES_PATH}/{applicationId}.asset");
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                templateListObject = Resources.Load<CharacterTemplateConfig>(applicationId);
            }
            var blueprints = await GetBlueprints(applicationId);
            var missingBlueprints = templateListObject.Templates == null || templateListObject.Templates.Length == 0  ? blueprints : blueprints.Where(blueprint => templateListObject.Templates.All(template => template.BlueprintId != blueprint.Id)).ToArray();
            if (missingBlueprints.Length == 0) return;
            
            var missingTemplates = await LoadAndCreateCharacterTemplates(missingBlueprints);
            var list = new List<CharacterTemplate>();
            if (templateListObject.Templates != null && templateListObject.Templates.Length > 0)
            {
                list = new List<CharacterTemplate>(templateListObject.Templates);
            }
            list.AddRange(missingTemplates);
            if(list.Count == 0) return;
            templateListObject.Templates = list.ToArray();
            EditorUtility.SetDirty(templateListObject);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static async Task<CharacterTemplate[]> LoadAndCreateCharacterTemplates(CharacterBlueprint[] blueprints)
        {
           
            var templates = new List<CharacterTemplate>();
            foreach (var blueprint in blueprints)
            {
                var template = new CharacterTemplate(blueprint.Name, blueprint.Id);
                template.cacheBlueprintId = blueprint.Id ;
                var blueprintPrefab = new BlueprintPrefab();
                blueprintPrefab.Tags = new string[] {"Default"};
                template.Prefabs = new[] { blueprintPrefab };
                templates.Add(template);
            }
            return templates.ToArray();
        }

        private static async Task<GameObject> LoadBlueprintModel(CharacterBlueprint blueprint)
        {
            var fileApi = new FileApi();
            var bytes = await fileApi.DownloadFileIntoMemoryAsync(blueprint.CharacterModel.ModelUrl);
            var glbCache = new GlbCache("Character Blueprints");
            await glbCache.Save(bytes, blueprint.Id);
            var loadedAsset = glbCache.Load(blueprint.Id);
            return loadedAsset;
        }

        private static async Task<CharacterBlueprint[]> GetBlueprints(string applicationId)
        {
            var blueprintApi = new BlueprintApi();
            var blueprints = await blueprintApi.ListAsync(new BlueprintListRequest
            {
                ApplicationId = applicationId
            });
            return blueprints.Data;
        }

        private static void ValidateFolders()
        {
            if (!AssetDatabase.IsValidFolder("Assets/PlayerZero"))
                AssetDatabase.CreateFolder("Assets", "PlayerZero"); 
            if (!AssetDatabase.IsValidFolder("Assets/PlayerZero/Resources"))
                AssetDatabase.CreateFolder("Assets/PlayerZero", "Resources");
        }
    }
}