using System;
using System.Linq;
using System.Reflection;
using PlayerZero.Data;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace PlayerZero.Editor
{
    [CustomEditor(typeof(AvatarRenderConfig))]
    public class AvatarRenderConfigEditor : UnityEditor.Editor
    {

        private AvatarRenderConfig avatarRenderConfig;
        private VisualElement root;
        
        public override VisualElement CreateInspectorGUI()
        {
            root = new VisualElement
            {
                style =
                {
                    paddingTop = 5,
                    paddingBottom = 5,
                    paddingLeft = 5,
                    paddingRight = 5

                }
            };
            avatarRenderConfig = (AvatarRenderConfig)target;
            
            GenerateFields(FetchFields());
            if (avatarRenderConfig.Scene == SceneType.Custom)
            {
                CustomScene();
            }
            return root;
        }
        
        private FieldInfo[] FetchFields()
        {
            var properties = typeof(AvatarRenderConfig).GetFields(BindingFlags.Public | BindingFlags.Instance);
            return properties.Where(f => f.Name != "customScene").ToArray();
        }
        
        
        private void GenerateFields(FieldInfo[] properties)
        {

            foreach (var property in properties)
            {
                if (property.FieldType == typeof(RenderSizeLimitType))
                {
                    var sizeField = new EnumField(property.Name, (RenderSizeLimitType)property.GetValue(avatarRenderConfig));
                    sizeField.RegisterValueChangedCallback(evt =>
                    {
                        property.SetValue(avatarRenderConfig, evt.newValue);
                        Save();
                        ReloadOnSave(property.Name);
                    });
                    root.Add(sizeField);
                }
                else if (property.FieldType == typeof(AvatarQuality))
                {
                    var qualityField = new EnumField(property.Name, (AvatarQuality)property.GetValue(avatarRenderConfig));
                    qualityField.RegisterValueChangedCallback(evt =>
                    {
                        property.SetValue(avatarRenderConfig, evt.newValue);
                        Save();
                        ReloadOnSave(property.Name);
                    });
                    root.Add(qualityField);
                }
                else if (property.FieldType == typeof(Color))
                {
                    var colorField = new ColorField(property.Name)
                    {
                        value = (Color)property.GetValue(avatarRenderConfig)
                    };
                    colorField.RegisterValueChangedCallback(evt =>
                    {
                        property.SetValue(avatarRenderConfig, evt.newValue);
                        Save();
                        ReloadOnSave(property.Name);
                    });
                    root.Add(colorField);
                }
                else if (property.FieldType.IsEnum)
                {
                    var enumField = new EnumField(property.Name, (Enum)property.GetValue(avatarRenderConfig));
                    enumField.RegisterValueChangedCallback(evt =>
                    {
                        property.SetValue(avatarRenderConfig, evt.newValue);
                        Save();
                        ReloadOnSave(property.Name);
                    });
                    root.Add(enumField);
                }
                else if (property.FieldType == typeof(string))
                {
                    var textField = new TextField(property.Name)
                    {
                        value = (string)property.GetValue(avatarRenderConfig)
                    };
                    textField.RegisterValueChangedCallback(evt =>
                    {
                        property.SetValue(avatarRenderConfig, evt.newValue);
                        Save();
                        ReloadOnSave(property.Name);
                    });
                    root.Add(textField);
                }
                else if (property.FieldType == typeof(int))
                {
                    var intField = new IntegerField(property.Name)
                    {
                        value = (int)property.GetValue(avatarRenderConfig)
                    };
                    intField.RegisterValueChangedCallback(evt =>
                    {
                        property.SetValue(avatarRenderConfig, evt.newValue);
                        Save();
                        ReloadOnSave(property.Name);
                    });
                    root.Add(intField);
                }

            }
        }
        
        private void ReloadOnSave(string name)
        {
            if (name == "scene")
            {
                root.Clear();
                GenerateFields(FetchFields());

                if (avatarRenderConfig.Scene == SceneType.Custom)
                {
                    CustomScene();
                }
            }
        }
        
        private void CustomScene()
        {
            var customScene = new TextField("Custom Scene");
            customScene.RegisterValueChangedCallback(x =>
            {
                avatarRenderConfig.CustomScene = x.newValue;
                Save();
            });
            root.Add(customScene);
        }
        
        private void Save()
        {
            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(serializedObject.targetObject);
        }

    }
}
