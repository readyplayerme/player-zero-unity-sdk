using System.Linq;
using UnityEditor;
using UnityEngine;
using System.Threading.Tasks;
using PlayerZero.Editor.UI.Components;
using PlayerZero.Editor.UI.ViewModels;

namespace PlayerZero.Editor.UI.Views
{
    public class ApplicationManagementView
    {
        private readonly ApplicationManagementViewModel viewModel;
        private readonly SelectInput selectInput;
        private readonly TextInput textInput;
        private readonly CharacterBlueprintsView characterBlueprintsView;
        private Vector2 scrollPosition = Vector2.zero;
        private string applicationId;
        
        public ApplicationManagementView(ApplicationManagementViewModel viewModel)
        {
            this.viewModel = viewModel;
            selectInput = new SelectInput();
            textInput = new TextInput();

            var characterBlueprintsViewModel = new CharacterBlueprintListViewModel(viewModel.BlueprintApi, viewModel.Settings, this.viewModel.AnalyticsApi);
            characterBlueprintsView = new CharacterBlueprintsView(characterBlueprintsViewModel);
        }

        public async Task Init()
        {
            await viewModel.Init();
            
            selectInput.Init(
                viewModel.Applications
                    .ToList()
                    .Select(app => new Option()
                    {
                        Label = $"{app.Name}",
                        Value = app.Id,
                    })
                    .ToArray(),
                viewModel.Settings.ApplicationId
            );
            textInput.Init(viewModel.Settings.ApiKey);
            applicationId = viewModel.Settings.ApplicationId;
            await characterBlueprintsView.InitAsync();
        }

        public void Render()
        {
            var scrollViewScope = new GUILayout.ScrollViewScope(scrollPosition, false, false);
            scrollPosition = scrollViewScope.scrollPosition;

            GUILayout.Space(15);

            if (viewModel.Loading)
            {
                using (new GUILayout.HorizontalScope())
                {
                    GUILayout.FlexibleSpace();
                    GUILayout.Label("Loading...", new GUIStyle()
                    {
                        alignment = TextAnchor.MiddleCenter,
                        normal = new GUIStyleState()
                        {
                            textColor = Color.white
                        }
                    });
                    GUILayout.FlexibleSpace();
                }
                scrollViewScope.Dispose();
                return;
                
            }

            GUILayout.Label("Project Settings", new GUIStyle()
            {
                fontStyle = FontStyle.Bold,
                normal = new GUIStyleState()
                {
                    textColor = Color.white
                },
                margin = new RectOffset(10, 10, 0, 0),
                fontSize = 14
            });

            GUILayout.Label("Select the Player Zero application to link to project",
                new GUIStyle(GUI.skin.label)
                {
                    margin = new RectOffset(9, 10, 0, 0)
                });
            
            using (new GUILayout.HorizontalScope(new GUIStyle()
                   {
                       margin = new RectOffset(7, 7, 5, 0)
                   }))
            {
                selectInput.Render(  async (applicationId) =>
                {
                    viewModel.Settings.ApplicationId = applicationId;
                    EditorUtility.SetDirty(viewModel.Settings);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                
                    await characterBlueprintsView.InitAsync();
                });
            }

            if (!string.IsNullOrEmpty(viewModel.Error))
            {
                GUILayout.Label(viewModel.Error, new GUIStyle()
                {
                    normal = new GUIStyleState()
                    {
                        textColor = Color.red
                    },
                    margin = new RectOffset()
                    {
                        left = 10
                    }
                });
            }
            
            GUILayout.Space(20);
            
            viewModel.Settings.DefaultAvatarId =
                EditorGUILayout.TextField("Default Avatar Id", viewModel.Settings.DefaultAvatarId,
                    new GUIStyle(GUI.skin.textField)
                    {
                        margin = new RectOffset(10, 10, 0, 0)
                    });

            GUILayout.Space(20);
            
            GUILayout.Label("Game ID", new GUIStyle()
            {
                fontStyle = FontStyle.Bold,
                normal = new GUIStyleState()
                {
                    textColor = Color.white
                },
                margin = new RectOffset(10, 10, 0, 0),
                fontSize = 14
            });

            GUILayout.Label("Paste your Game ID here",
                new GUIStyle(GUI.skin.label)
                {
                    margin = new RectOffset(9, 10, 0, 0)
                });
            
            viewModel.Settings.GameId =
                EditorGUILayout.TextField("Game ID", viewModel.Settings.GameId,
                    new GUIStyle(GUI.skin.textField)
                    {
                        margin = new RectOffset(10, 10, 0, 0)
                    });
            
            GUILayout.Space(20);

            characterBlueprintsView.Render();

            GUILayout.Space(20);
            
            GUILayout.Label("Auth Settings", new GUIStyle()
            {
                fontStyle = FontStyle.Bold,
                normal = new GUIStyleState()
                {
                    textColor = Color.white
                },
                margin = new RectOffset(10, 10, 0, 0),
                fontSize = 14
            });

            using (new GUILayout.VerticalScope(new GUIStyle()
                   {
                       margin = new RectOffset(7, 7, 5, 0)
                   }))
            {
                viewModel.Settings.ApiProxyUrl =
                    EditorGUILayout.TextField("Proxy Api Url", viewModel.Settings.ApiProxyUrl);

                GUILayout.Space(5);

                textInput.Render("Api Key", (value) =>
                {
                    viewModel.Settings.ApiKey = value;
                    EditorUtility.SetDirty(viewModel.Settings);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                });

                GUILayout.Space(5);

                EditorGUILayout.HelpBox(
                    "Setting your API Key in the field above can be insecure as it means that your API Key can be discoverable in your game build, it is advisable to instead setup a proxy server. See our docs for more details.",
                    MessageType.Info
                );
            }

            GUILayout.Space(20);
            scrollViewScope.Dispose();
        }
    }
}