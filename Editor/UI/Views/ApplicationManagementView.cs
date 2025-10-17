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
        private readonly AnalyticsView analyticsView;
        private Vector2 scrollPosition = Vector2.zero;
        private string applicationId;
        private int toolbarInt = 0;
        
        public ApplicationManagementView(ApplicationManagementViewModel viewModel)
        {
            this.viewModel = viewModel;
            selectInput = new SelectInput();
            textInput = new TextInput();

            var characterBlueprintsViewModel = new CharacterBlueprintListViewModel(viewModel.BlueprintApi, viewModel.Settings, this.viewModel.AnalyticsApi);
            characterBlueprintsView = new CharacterBlueprintsView(characterBlueprintsViewModel);
            analyticsView = new AnalyticsView();
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
            GUILayout.Space(5);
            viewModel.Settings.GameId =
                EditorGUILayout.TextField("Game ID", viewModel.Settings.GameId,
                    new GUIStyle(GUI.skin.textField)
                    {
                        margin = new RectOffset(10, 10, 0, 0)
                    });
            
            GUILayout.Space(10);
            
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace(); 

            GUILayoutOption[] textFieldOptions = { GUILayout.Width(200) };
            string[] toolbarStrings = { "Blueprints", "Analytics" };
            toolbarInt = GUILayout.Toolbar(toolbarInt, toolbarStrings, textFieldOptions);

            GUILayout.FlexibleSpace(); 
            GUILayout.EndHorizontal();
            GUILayout.Space(10);
            
            switch (toolbarInt)
            {
                case 0:
                    characterBlueprintsView.Render();
                    break;
                case 1:
                    analyticsView.Render(viewModel.Settings.GameId, viewModel.Settings.DefaultAvatarId);
                    break;
            }

            GUILayout.Space(20);
            
            scrollViewScope.Dispose();
        }
    }
}