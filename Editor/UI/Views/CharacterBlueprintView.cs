using System.Linq;
using PlayerZero.Data;
using System.Threading.Tasks;
using PlayerZero.Api.V1;
using PlayerZero.Editor.UI.Components;
using PlayerZero.Editor.UI.ViewModels;
using UnityEditor;
using UnityEngine;

namespace PlayerZero.Editor.UI.Views
{
    public class CharacterBlueprintView
    {
        private readonly CharacterBlueprintViewModel _viewModel;
        private string characterBlueprintId;
        public CharacterBlueprintView(CharacterBlueprintViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public async Task Init(CharacterBlueprint characterBlueprint)
        {
            await _viewModel.Init(characterBlueprint);
            characterBlueprintId = characterBlueprint.Id;
        }

        public void Render()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                using (new EditorGUILayout.VerticalScope(new GUIStyle()
                       {
                           fixedWidth = 120
                       }))
                {
                    using (new EditorGUILayout.HorizontalScope(new GUIStyle()
                           {
                               normal = new GUIStyleState()
                               {
                                   background = Texture2D.grayTexture,
                               },
                               margin = new RectOffset(5, 5, 5, 5),
                           }))
                    {
                        GUILayout.FlexibleSpace();
                        GUILayout.Label(_viewModel.Image,
                            new GUIStyle()
                            {
                                stretchWidth = true,
                                stretchHeight = true,
                                fixedWidth = 90,
                                fixedHeight = 90,
                                alignment = TextAnchor.MiddleCenter,
                            });
                        GUILayout.FlexibleSpace();
                    }

                    if (GUILayout.Button("Load Blueprint"))
                    {
#pragma warning disable CS4014
                        _viewModel.LoadBlueprintAsync();
#pragma warning restore CS4014
                    }
                }

                using (new EditorGUILayout.VerticalScope())
                {
                    EditorGUILayout.LabelField("ID: " + _viewModel.CharacterBlueprint.Id, EditorStyles.label);
                    GUILayout.Space(3); 
                    EditorGUILayout.LabelField("Default Template Prefab", EditorStyles.boldLabel);
                }
            }
        }
    }
}