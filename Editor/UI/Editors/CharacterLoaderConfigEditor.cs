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
        private const string DIALOG_TITLE = "Read Player Me";
        private const string DIALOG_MESSAGE = "Do you want to install {0} Unity Package: {1} ?";
        private const string DIALOG_OK = "Ok";
        private const string DIALOG_CANCEL = "Cancel";
        private const string ADD_MORPH_TARGET = "Add Morph Target";
        private const string DELETE_MORPH_TARGET = "Delete Morph Target";
        private const string REMOVE_BUTTON_TEXT = "X";
        private const string MESH_OPT_PACKAGE_NAME = "com.unity.meshopt.decompress";
        
        [SerializeField] private VisualTreeAsset visualTreeAsset;
        
        private CharacterLoaderConfig characterLoaderConfigTarget;
        private List<Label> morphTargetLabels;

        private Dictionary<VisualElement, string> morphTargetsParentVisualElement;

        private VisualElement selectedMorphTargets;
        private VisualElement selectedMorphTargetGroups;
        
        private EditableList editableMorphTargetList;
        private EditableList editableMorphTargetGroupList;

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
            SetupMorphTargetGroups();
            SetupCompressionPackages();
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
        
        private void SetupCompressionPackages()
        {
            var optimizationFoldout = new Foldout
            {
                text = "Optimization Packages",
                value = false
            };
            optimizationFoldout.style.marginTop = 4;
            optimizationFoldout.style.marginLeft = 4;
            optimizationFoldout.style.marginRight = 4;
            optimizationFoldout.style.flexShrink = 0;

            // Create the "Use Draco Compression" toggle
            var dracoToggle = new Toggle("Use Draco Compression");
            dracoToggle.name = "UseDracoCompression";
            dracoToggle.style.marginLeft = 3;
            dracoToggle.style.marginTop = 3;
            dracoToggle.style.flexDirection = FlexDirection.Row;
            dracoToggle.style.alignItems = Align.Center;
            dracoToggle.style.justifyContent = Justify.FlexStart;

            // Create the "Use Mesh Opt Compression" toggle
            var meshOptToggle = new Toggle("Use Mesh Opt Compression");
            meshOptToggle.name = "UseMeshOptCompression";
            meshOptToggle.style.marginLeft = 3;
            meshOptToggle.style.marginTop = 3;
            meshOptToggle.style.flexDirection = FlexDirection.Row;
            meshOptToggle.style.alignItems = Align.Center;
            meshOptToggle.style.flexGrow = 0;
            meshOptToggle.style.flexShrink = 0;
            meshOptToggle.style.flexWrap = Wrap.NoWrap;
            meshOptToggle.style.position = Position.Relative;
            meshOptToggle.style.overflow = Overflow.Hidden;
            meshOptToggle.style.opacity = 1;

            // Add toggles to the foldout
            optimizationFoldout.Add(dracoToggle);
            optimizationFoldout.Add(meshOptToggle);

            // Add the foldout to the root or desired parent
            root.Add(optimizationFoldout);
            
            optimizationFoldout.RegisterValueChangedCallback(x =>
            {
                dracoToggle.SetValueWithoutNotify(characterLoaderConfigTarget.UseDracoCompression);
                meshOptToggle.SetValueWithoutNotify(characterLoaderConfigTarget.UseMeshOptCompression);
            });

            dracoToggle.RegisterValueChangedCallback(x =>
                {
                    if (characterLoaderConfigTarget.UseDracoCompression == x.newValue) return;
                    characterLoaderConfigTarget.UseDracoCompression = x.newValue;
                    if (!PackageManagerHelper.IsPackageInstalled(PackageList.DracoCompression.name))
                    {
                        if (EditorUtility.DisplayDialog(
                                DIALOG_TITLE,
                                string.Format(DIALOG_MESSAGE, "Draco compression", PackageList.DracoCompression.name),
                                DIALOG_OK,
                                DIALOG_CANCEL))
                        {
                            PackageManagerHelper.AddPackage(PackageList.DracoCompression.Identifier);
                        }
                        else
                        {
                            characterLoaderConfigTarget.UseDracoCompression = false;
                        }
                    }
                    dracoToggle.SetValueWithoutNotify(characterLoaderConfigTarget.UseDracoCompression);
                    if (characterLoaderConfigTarget.UseDracoCompression && characterLoaderConfigTarget.UseMeshOptCompression)
                    {
                        Debug.LogWarning("Draco compression is not compatible with Mesh Optimization compression. Mesh Optimization compression will be disabled.");
                        characterLoaderConfigTarget.UseMeshOptCompression = false;
                        dracoToggle.SetValueWithoutNotify(false);
                    }
                    Save();
                }
            );

            meshOptToggle.RegisterValueChangedCallback(x =>
                {
                    if (characterLoaderConfigTarget.UseMeshOptCompression == x.newValue) return;
                    characterLoaderConfigTarget.UseMeshOptCompression = x.newValue;
                    if (!PackageManagerHelper.IsPackageInstalled(MESH_OPT_PACKAGE_NAME))
                    {
                        if (EditorUtility.DisplayDialog(
                                DIALOG_TITLE,
                                string.Format(DIALOG_MESSAGE, "Mesh opt compression", MESH_OPT_PACKAGE_NAME),
                                DIALOG_OK,
                                DIALOG_CANCEL))
                        {
                            PackageManagerHelper.AddPackage(MESH_OPT_PACKAGE_NAME);
                        }
                        else
                        {
                            characterLoaderConfigTarget.UseMeshOptCompression = false;
                        }
                    }
                    meshOptToggle.SetValueWithoutNotify(characterLoaderConfigTarget.UseMeshOptCompression);
                    if (characterLoaderConfigTarget.UseMeshOptCompression && characterLoaderConfigTarget.UseDracoCompression)
                    {
                        Debug.LogWarning("Mesh Optimization compression is not compatible with Draco compression. Draco compression will be disabled.");
                        characterLoaderConfigTarget.UseDracoCompression = false;
                        meshOptToggle.SetValueWithoutNotify(false);
                    }
                    Save();
                }
            );
        }
        
        private void SetupMorphTargets()
        {
            editableMorphTargetList = new EditableList(AvatarMorphTargets.MorphTargetNames);
            // Main container
            var morphFoldout = new Foldout { text = "Morph Targets" };
            root.Add(morphFoldout);
            
            selectedMorphTargets = editableMorphTargetList.GetRoot();
            morphFoldout.Add(selectedMorphTargets);
            
            for (var i = 0; i < characterLoaderConfigTarget.MorphTargets.Count; i++)
            {
                
                var defaultIndex = Array.IndexOf(AvatarMorphTargets.MorphTargetNames,characterLoaderConfigTarget.MorphTargets[i]);
                var newElement = editableMorphTargetList.CreateNewElement(defaultIndex,
                    label =>
                    {
                        characterLoaderConfigTarget.MorphTargets[GetIndex(label.text)] = label.text;
                        return null;
                    }, 
                    removedElement =>
                    {
                        Undo.RecordObject(characterLoaderConfigTarget, DELETE_MORPH_TARGET);
                        characterLoaderConfigTarget.MorphTargets.RemoveAt(GetIndex(removedElement));
                        EditorUtility.SetDirty(characterLoaderConfigTarget);
                    });
            }

            // Add button
            var addButton = new Button(OnAddMorphTargetButtonClicked) { text = "Add" };
            morphFoldout.Add(addButton);
        }
        
        private void SetupMorphTargetGroups()
        {
            editableMorphTargetGroupList = new EditableList(AvatarMorphTargets.MorphTargetGroupNames);
            // Main container
            var morphFoldout = new Foldout { text = "Morph Target Groups" };
            root.Add(morphFoldout);

            // VisualElement to hold all selected morph target rows
            selectedMorphTargetGroups = editableMorphTargetGroupList.GetRoot();
            morphFoldout.Add(selectedMorphTargetGroups);

            for (var i = 0; i < characterLoaderConfigTarget.MorphTargetsGroup.Count; i++)
            {
                var defaultIndex = Array.IndexOf(AvatarMorphTargets.MorphTargetGroupNames,characterLoaderConfigTarget.MorphTargetsGroup[i]);
                var newElement = editableMorphTargetGroupList.CreateNewElement(defaultIndex,
                    label =>
                    {
                        characterLoaderConfigTarget.MorphTargetsGroup[GetIndex(label.text)] = label.text;
                        return null;
                    }, 
                    removedElement =>
                    {
                        Undo.RecordObject(characterLoaderConfigTarget, DELETE_MORPH_TARGET);
                        characterLoaderConfigTarget.MorphTargetsGroup.RemoveAt(GetIndex(removedElement));
                        EditorUtility.SetDirty(characterLoaderConfigTarget);
                    });
            }

            // Add button
            var addButton = new Button(OnAddMorphTargetGroupButtonClicked) { text = "Add" };
            morphFoldout.Add(addButton);
        }
        
        private void OnAddMorphTargetButtonClicked()
        {
            Undo.RecordObject(characterLoaderConfigTarget, ADD_MORPH_TARGET);
            characterLoaderConfigTarget.MorphTargets.Add(AvatarMorphTargets.MorphTargetNames[0]);
            EditorUtility.SetDirty(characterLoaderConfigTarget);
            editableMorphTargetList.CreateNewElement(0);
        }

        private void OnAddMorphTargetGroupButtonClicked()
        {
            Undo.RecordObject(characterLoaderConfigTarget, ADD_MORPH_TARGET);
            characterLoaderConfigTarget.MorphTargetsGroup.Add(AvatarMorphTargets.MorphTargetGroupNames[0]);
            EditorUtility.SetDirty(characterLoaderConfigTarget);
            editableMorphTargetGroupList.CreateNewElement(0);
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
