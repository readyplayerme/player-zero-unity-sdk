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

        private const string TOOLTIP_MESH_LOD = "Mesh LOD is used to determine the level of detail for the mesh. LOD0 is the most detailed.";
        private const string TOOLTIP_TEXTURE_ATLAS = "If set to NONE the mesh, materials and textures will not be combined into 1. (or 2 if an assets texture contains transparency)";
        private const string TOOLTIP_TEXTURE_QUALITY = "Texture Quality is used to determine the quality of the textures. High quality textures will be larger in size.";
        private const string TOOLTIP_TEXTURE_CHANNEL = "Choose which textures the avatar will include.";
        private const string TOOLTIP_OPTIMIZATION = "Enable or disable optimization for the avatar model.";
        private const string TOOLTIP_DRACO = "If true, the mesh will be compressed using Draco compression.";
        private const string TOOLTIP_MESH_OPT = "If true, the mesh will be compressed using meshoptimizer compression. (Experimental)";
        private const string TOOLTIP_MORPH_TARGETS = "Add morph targets are used to create facial expressions and other deformations. Otherwise, set to None.";
        private const string TOOLTIP_MORPH_TARGET_GROUPS = "Add morph target groups to include a set of morph targets.";
        
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

            MeshLod();
            TextureAtlas();
            TextureQuality();
            TextireSizeLimit();
            TextureChannels();
            MorphTargets();
            MorphTargetGroups();
            CompressionPackages();
            return root;
        }
        
        private void Save()
        {
            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(serializedObject.targetObject);
        }
        
        private void MeshLod()
        {
            var lodField = new EnumField("Mesh LOD", characterLoaderConfigTarget.MeshLOD);
            lodField.tooltip = TOOLTIP_MESH_LOD;
            lodField.RegisterValueChangedCallback(x =>
            {
                characterLoaderConfigTarget.MeshLOD = (MeshLod)x.newValue;
                Save();
            });
            root.Add(lodField);
        }
        
        private void TextureAtlas()
        {
            var field = new EnumField("Texture Atlas", characterLoaderConfigTarget.TextureAtlas);
            field.tooltip =TOOLTIP_TEXTURE_ATLAS;
            field.RegisterValueChangedCallback(x =>
            {
                characterLoaderConfigTarget.TextureAtlas = (TextureAtlas)x.newValue;
                Save();
            });
            root.Add(field);
        }
        
        private void TextureChannels()
        {
            var items = Enum.GetNames(typeof(TextureChannel)).ToList();
            var container = new Foldout
            {
                text = "Texture Channels", 
                tooltip = TOOLTIP_TEXTURE_CHANNEL,
                style =
                {
                    marginTop = 5
                }
            };

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
        
        private void TextureQuality()
        {
            var textureQualityField = new EnumField("Texture Quality", characterLoaderConfigTarget.TextureQuality);
            textureQualityField.tooltip = TOOLTIP_TEXTURE_QUALITY;
            textureQualityField.RegisterValueChangedCallback(x =>
            {
                characterLoaderConfigTarget.TextureQuality = (TextureQuality)x.newValue;
                Save();
            });
            root.Add(textureQualityField);
        }
        
        private void TextireSizeLimit()
        {
            var sizeLimitField = new EnumField("Texture Size Limit", characterLoaderConfigTarget.TextureSizeLimit);
            sizeLimitField.tooltip = "The maximum size of the texture in pixels.";
            sizeLimitField.RegisterValueChangedCallback(x =>
            {
                characterLoaderConfigTarget.TextureSizeLimit = (TextureSizeLimit)x.newValue;
                Save();
            });
            root.Add(sizeLimitField);
        }
        
        private void CompressionPackages()
        {
            var optimizationFoldout = new Foldout
            {
                text = "Optimization Packages",
                value = false,
                tooltip = TOOLTIP_OPTIMIZATION,
                style =
                {
                    marginTop = 4,
                    marginLeft = 4,
                    marginRight = 4,
                    flexShrink = 0
                }
            };

            // Create the "Use Draco Compression" toggle
            var dracoToggle = new Toggle("Draco Compression")
            {
                tooltip = TOOLTIP_DRACO,
                name = "Draco Compression",
                style =
                {
                    marginLeft = 3,
                    marginTop = 3,
                    flexDirection = FlexDirection.Row,
                    alignItems = Align.Center,
                    justifyContent = Justify.FlexStart
                }
            };

            // Create the "Use Mesh Opt Compression" toggle
            var meshOptToggle = new Toggle("Mesh Compression (Experimental)")
            {
                tooltip = TOOLTIP_MESH_OPT,
                name = "Mesh Compression",
                style =
                {
                    marginLeft = 3,
                    marginTop = 3,
                    flexDirection = FlexDirection.Row,
                    alignItems = Align.Center,
                    flexGrow = 0,
                    flexShrink = 0,
                    flexWrap = Wrap.NoWrap,
                    position = Position.Relative,
                    overflow = Overflow.Hidden,
                    opacity = 1
                }
            };

            // Add toggles to the foldout
            optimizationFoldout.Add(dracoToggle);
            optimizationFoldout.Add(meshOptToggle);

            // Add the foldout to the root or desired parent
            root.Add(optimizationFoldout);
            
            optimizationFoldout.RegisterValueChangedCallback(x =>
            {
                dracoToggle.SetValueWithoutNotify(characterLoaderConfigTarget.DracoCompression);
                meshOptToggle.SetValueWithoutNotify(characterLoaderConfigTarget.MeshCompression);
            });

            dracoToggle.RegisterValueChangedCallback(x =>
                {
                    if (characterLoaderConfigTarget.DracoCompression == x.newValue) return;
                    characterLoaderConfigTarget.DracoCompression = x.newValue;
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
                            characterLoaderConfigTarget.DracoCompression = false;
                        }
                    }
                    dracoToggle.SetValueWithoutNotify(characterLoaderConfigTarget.DracoCompression);
                    if (characterLoaderConfigTarget.DracoCompression && characterLoaderConfigTarget.MeshCompression)
                    {
                        Debug.LogWarning("Draco compression is not compatible with Mesh Optimization compression. Mesh Optimization compression will be disabled.");
                        characterLoaderConfigTarget.MeshCompression = false;
                        dracoToggle.SetValueWithoutNotify(false);
                    }
                    Save();
                }
            );

            meshOptToggle.RegisterValueChangedCallback(x =>
                {
                    if (characterLoaderConfigTarget.MeshCompression == x.newValue) return;
                    characterLoaderConfigTarget.MeshCompression = x.newValue;
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
                            characterLoaderConfigTarget.MeshCompression = false;
                        }
                    }
                    meshOptToggle.SetValueWithoutNotify(characterLoaderConfigTarget.MeshCompression);
                    if (characterLoaderConfigTarget.MeshCompression && characterLoaderConfigTarget.DracoCompression)
                    {
                        Debug.LogWarning("Mesh Optimization compression is not compatible with Draco compression. Draco compression will be disabled.");
                        characterLoaderConfigTarget.DracoCompression = false;
                        meshOptToggle.SetValueWithoutNotify(false);
                    }
                    Save();
                }
            );
        }
        
        private void MorphTargets()
        {
            editableMorphTargetList = new EditableList(AvatarMorphTargets.MorphTargetNames);
            characterLoaderConfigTarget.MorphTargets ??= new List<string>();
            var morphFoldout = new Foldout
            {
                text = "Morph Targets", 
                tooltip = TOOLTIP_MORPH_TARGETS
            };
            
            root.Add(morphFoldout);
            
            selectedMorphTargets = editableMorphTargetList.GetRoot();
            morphFoldout.Add(selectedMorphTargets);

            for (int i = 0; i < characterLoaderConfigTarget.MorphTargets.Count; i++)
            {
                var morphTarget = characterLoaderConfigTarget.MorphTargets[i];
                var defaultIndex = Array.IndexOf(AvatarMorphTargets.MorphTargetNames, morphTarget);

                var newElement = editableMorphTargetList.CreateNewElement(defaultIndex,
                    label =>
                    {
                        var index = GetIndex(characterLoaderConfigTarget.MorphTargets, label.text);
                        if (index >= 0)
                        {
                            characterLoaderConfigTarget.MorphTargets[index] = label.text;
                        }
                        return null;
                    },
                    removedElement =>
                    {
                        Undo.RecordObject(characterLoaderConfigTarget, DELETE_MORPH_TARGET);
                        var index = GetIndex(characterLoaderConfigTarget.MorphTargets, removedElement);
                        if (index >= 0)
                        {
                            characterLoaderConfigTarget.MorphTargets.RemoveAt(index);
                        }
                        EditorUtility.SetDirty(characterLoaderConfigTarget);
                    });
            }

            // Add button
            var addButton = new Button(OnAddMorphTargetButtonClicked)
            {
                text = "Add"
            };
            morphFoldout.Add(addButton);
        }
        
        private void MorphTargetGroups()
        {
            editableMorphTargetGroupList = new EditableList(AvatarMorphTargets.MorphTargetGroupNames);
            characterLoaderConfigTarget.MorphTargetsGroup ??= new List<string>();

            var morphFoldout = new Foldout
            {
                text = "Morph Target Groups", 
                tooltip = TOOLTIP_MORPH_TARGET_GROUPS
            };
            root.Add(morphFoldout);

            // VisualElement to hold all selected morph target rows
            selectedMorphTargetGroups = editableMorphTargetGroupList.GetRoot();
            morphFoldout.Add(selectedMorphTargetGroups);

            for (int i = 0; i < characterLoaderConfigTarget.MorphTargetsGroup.Count; i++)
            {
                var morphTargetGroup = characterLoaderConfigTarget.MorphTargetsGroup[i];
                var defaultIndex = Array.IndexOf(AvatarMorphTargets.MorphTargetGroupNames, morphTargetGroup);

                var newElement = editableMorphTargetGroupList.CreateNewElement(defaultIndex,
                    label =>
                    {
                        var index = GetIndex(characterLoaderConfigTarget.MorphTargetsGroup, label.text);
                        if (index >= 0)
                        {
                            characterLoaderConfigTarget.MorphTargetsGroup[index] = label.text;
                        }
                        return null;
                    },
                    removedElement =>
                    {
                        Undo.RecordObject(characterLoaderConfigTarget, DELETE_MORPH_TARGET);
                        
                        int index = GetIndex(characterLoaderConfigTarget.MorphTargetsGroup, removedElement);
                        if (index >= 0)
                        {
                            characterLoaderConfigTarget.MorphTargetsGroup.RemoveAt(index);
                        }
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
        
        private int GetIndex(List<string> list, string value)
        {
            return list.FindIndex(x => x == value);
        }

        private void UpdateMorphTargetListFromUI()
        {
            characterLoaderConfigTarget.MorphTargets = morphTargetsParentVisualElement
                .Values
                .ToList();
        }
    }
}
