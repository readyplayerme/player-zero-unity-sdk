using System;
using System.Collections.Generic;
using System.Linq;
using PlayerZero.Data;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace PlayerZero.Editor
{
    [CustomEditor(typeof(CharacterLoaderConfig))]
    public class CharacterLoaderConfigEditor : UnityEditor.Editor
    {
        private const string ADD_MORPH_TARGET = "Add Morph Target";
        private const string DELETE_MORPH_TARGET = "Delete Morph Target";
        private const string REMOVE_BUTTON_TEXT = "X";
        
        [SerializeField] private VisualTreeAsset visualTreeAsset;
        
        private CharacterLoaderConfig characterLoaderConfigTarget;
        private List<Label> morphTargetLabels;

        private Dictionary<VisualElement, string> morphTargetsParentVisualElement;

        private VisualElement selectedMorphTargets;
        private VisualElement selectedMorphTargetGroups;

        private VisualElement root;
        private Action textureChannelChanged;

        private SerializedProperty shaderPropertyMappingList;
        
        public override VisualElement CreateInspectorGUI()
        {
            root = new VisualElement
            {
                style = { paddingTop = 5, paddingBottom = 5, paddingLeft = 5, paddingRight = 5 }
            };

            characterLoaderConfigTarget = (CharacterLoaderConfig)target;

            SetupLod();
            SetupTextureAtlas();
            SetupTextureChannel();
            SetupMorphTargets();

            return root;
        }
        
        private void Save()
        {
            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(serializedObject.targetObject);
        }
        
        private void SetupLod()
        {
            var lodField = new EnumField("Mesh LOD", characterLoaderConfigTarget.MeshLod);
            lodField.RegisterValueChangedCallback(x =>
            {
                characterLoaderConfigTarget.MeshLod = (MeshLod)x.newValue;
                Save();
            });
            root.Add(lodField);
        }
        
        private void SetupTextureAtlas()
        {
            var field = new EnumField("Texture Atlas", characterLoaderConfigTarget.TextureAtlas);
            field.RegisterValueChangedCallback(x =>
            {
                characterLoaderConfigTarget.TextureAtlas = (TextureAtlas)x.newValue;
                Save();
            });
            root.Add(field);
        }
        
        private void SetupTextureChannel()
        {
            var items = Enum.GetNames(typeof(TextureChannel)).ToList();
            var container = new Foldout { text = "Texture Channels" };
            container.style.marginTop = 5;

            for (var i = 0; i < items.Count; i++)
            {
                var index = i;
                var textureChannel = (TextureChannel)index;
                var toggle = new Toggle(items[i])
                {
                    value = characterLoaderConfigTarget.TextureChannel.Contains(textureChannel)
                };

                toggle.RegisterValueChangedCallback(x =>
                {
                    var channels = characterLoaderConfigTarget.TextureChannel.ToList();
                    if (x.newValue && !channels.Contains(textureChannel))
                        channels.Add(textureChannel);
                    else if (!x.newValue && channels.Contains(textureChannel))
                        channels.Remove(textureChannel);

                    characterLoaderConfigTarget.TextureChannel = channels.ToArray();
                    textureChannelChanged?.Invoke();
                    Save();
                });

                container.Add(toggle);
            }

            root.Add(container);
        }
        
        private void SetupMorphTargets()
        {
            // Main container
            var morphFoldout = new Foldout { text = "Morph Targets" };
            root.Add(morphFoldout);

            // VisualElement to hold all selected morph target rows
            selectedMorphTargets = new VisualElement();
            morphFoldout.Add(selectedMorphTargets);

            morphTargetLabels = AvatarMorphTargets.MorphTargetNames.Select(x => new Label(x)).ToList();
            morphTargetsParentVisualElement = new Dictionary<VisualElement, string>();

            for (var i = 0; i < characterLoaderConfigTarget.MorphTargets.Count; i++)
            {
                var defaultIndex = AvatarMorphTargets.MorphTargetNames.IndexOf(characterLoaderConfigTarget.MorphTargets[i]);
                CreateNewElement(defaultIndex);
            }

            // Add button
            var addButton = new Button(OnAddButtonClicked) { text = "Add" };
            morphFoldout.Add(addButton);
        }
        
        private void CreateNewElement(int popFieldDefaultIndex)
        {
            var parent = new VisualElement();
            parent.style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row);
            parent.style.justifyContent = new StyleEnum<Justify>(Justify.SpaceBetween);
            Debug.Log($" Creating new morph target element with default index: {popFieldDefaultIndex}");
            morphTargetsParentVisualElement.Add(parent, AvatarMorphTargets.MorphTargetNames[popFieldDefaultIndex]);
            parent.Add(CreatePopupField(popFieldDefaultIndex, parent));
            parent.Add(CreateRemoveButton(parent));
            selectedMorphTargets.Add(parent);
        }

        private PopupField<Label> CreatePopupField(int defaultIndex, VisualElement parent)
        {
            return new PopupField<Label>(string.Empty,
                morphTargetLabels,
                defaultIndex,
                x =>
                {
                    characterLoaderConfigTarget.MorphTargets[GetIndex(morphTargetsParentVisualElement[parent])] = x.text;
                    morphTargetsParentVisualElement[parent] = x.text;
                    return x.text;
                },
                x => x.text);
        }

        private VisualElement CreateRemoveButton(VisualElement parent)
        {
            var removeButton = new Button(() =>
            {
                Undo.RecordObject(characterLoaderConfigTarget, DELETE_MORPH_TARGET);
                characterLoaderConfigTarget.MorphTargets.RemoveAt(GetIndex(morphTargetsParentVisualElement[parent]));
                selectedMorphTargets.Remove(parent);
                EditorUtility.SetDirty(characterLoaderConfigTarget);
            });
            removeButton.text = REMOVE_BUTTON_TEXT;
            return removeButton;
        }

        private void OnAddButtonClicked()
        {
            Undo.RecordObject(characterLoaderConfigTarget, ADD_MORPH_TARGET);
            characterLoaderConfigTarget.MorphTargets.Add(AvatarMorphTargets.MorphTargetNames[0]);
            EditorUtility.SetDirty(characterLoaderConfigTarget);
            CreateNewElement(0);
        }

        private int GetIndex(string morphTarget)
        {
            return characterLoaderConfigTarget.MorphTargets.FindIndex(x => x == morphTarget);
        }

        private void UpdateMorphTargetListFromUI()
        {
            characterLoaderConfigTarget.MorphTargets = morphTargetsParentVisualElement
                .Values
                .ToList();
        }
    }
}
