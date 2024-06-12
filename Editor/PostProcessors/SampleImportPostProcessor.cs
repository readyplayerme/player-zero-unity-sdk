﻿using System;
using System.Collections.Generic;
using System.Linq;
using ReadyPlayerMe.Data;
using ReadyPlayerMe.Editor.Api.V1.Analytics;
using ReadyPlayerMe.Editor.Api.V1.Analytics.Models;
using ReadyPlayerMe.Editor.Cache;
using UnityEditor;
using UnityEngine;

namespace ReadyPlayerMe.Editor.PostProcessors
{
    public class SampleImportPostProcessor : AssetPostprocessor
    {
        public const string DemoCharacterStyleId = "665e05e758e847063761c985";
        
        private static void OnPostprocessAllAssets(
            string[] importedAssets,
            string[] deletedAssets,
            string[] movedAssets,
            string[] movedFromAssetPaths
            )
        {
            if (importedAssets.FirstOrDefault(p => p.Contains("Character Clothing Example")) != null)
            {
                var characterStyleConfigCache = new ScriptableObjectCache<CharacterStyleTemplateConfig>();
                var characterStyleConfig = characterStyleConfigCache.Init("CharacterStyleTemplateConfig");
                var templates = characterStyleConfig.templates?.ToList() ?? new List<CharacterStyleTemplate>();
                var existingTemplate = templates.FirstOrDefault(p => p.tags.Contains(DemoCharacterStyleId));
                if (existingTemplate == null)
                {
                    var matchingTemplates = AssetDatabase.FindAssets("RPM_Character_Template");
                    var templatePath = AssetDatabase.GUIDToAssetPath(matchingTemplates[0]);
                    var templateAsset = AssetDatabase.LoadAssetAtPath<GameObject>(templatePath);

                    templates.Add(new CharacterStyleTemplate()
                    {
                        id = Guid.NewGuid().ToString(),
                        cacheId = matchingTemplates[0],
                        tags = new List<string>() { DemoCharacterStyleId },
                        template = templateAsset
                    });
                }
            
                characterStyleConfig.templates = templates.ToArray();
                
                EditorUtility.SetDirty(characterStyleConfig);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                var analyticsApi = new AnalyticsApi();
                analyticsApi.SendEvent(new AnalyticsEventRequest()
                {
                    Payload = new AnalyticsEventRequestBody()
                    {
                        Event = "next gen unity sdk action",
                        Properties =
                        {
                            { "type", "Custom Clothing Sample Imported" }
                        }
                    }
                });
            }
        }
    }
}