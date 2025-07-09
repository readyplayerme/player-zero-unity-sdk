using PlayerZero.Api.V1;
using PlayerZero.Data;
using PlayerZero.Editor.Api.V1.Analytics;
using PlayerZero.Editor.Api.V1.Auth;
using PlayerZero.Editor.Api.V1.DeveloperAccounts;
using PlayerZero.Editor.Cache;
using PlayerZero.Editor.Cache.EditorPrefs;
using PlayerZero.Editor.UI.ViewModels;
using PlayerZero.Editor.UI.Views;
using UnityEditor;
using UnityEngine;

namespace PlayerZero.Editor.UI.Windows
{
    public class PlayerZeroEditor : EditorWindow
    {
        private DeveloperLoginView developerLoginView;
        private ApplicationManagementView applicationManagementView;
        
        [MenuItem("Tools/Player Zero", false, 0)]
        public static void Generate()
        {
            var window = GetWindow<PlayerZeroEditor>("Player Zero");
            window.minSize = new Vector2(700, 120);
        }

        private async void OnEnable()
        {
            var developerAuthApi = new DeveloperAuthApi();
            var developerAccountApi = new DeveloperAccountApi();
            var blueprintApi = new BlueprintApi();
            var analyticsApi = new AnalyticsApi();

            var settingsCache = new ScriptableObjectCache<Settings>();
            var settings = settingsCache.Init("PlayerZeroSettings");

            var developerLoginViewModel = new DeveloperLoginViewModel(developerAuthApi, analyticsApi);
            developerLoginView = new DeveloperLoginView(developerLoginViewModel);

            var projectDetailsViewModel = new ApplicationManagementViewModel(
                analyticsApi,
                blueprintApi,
                developerAccountApi,
                settings
            );

            applicationManagementView = new ApplicationManagementView(projectDetailsViewModel);
            if (DeveloperAuthCache.Exists())
                await applicationManagementView.Init();
        }

        private void OnGUI()
        {
            if (!DeveloperAuthCache.Exists())
            {
                developerLoginView.Render(async () => { await applicationManagementView.Init(); });
                return;
            }

            using (new GUILayout.HorizontalScope())
            {
                GUILayout.Label($"Welcome {DeveloperAuthCache.Data.Name},", new GUIStyle()
                {
                    normal = new GUIStyleState()
                    {
                        textColor = Color.white
                    },
                    margin = new RectOffset(5, 5, 7, 5),
                });

                if (GUILayout.Button("Sign Out", new GUIStyle()
                    {
                        fixedWidth = 70,
                        normal = new GUIStyleState()
                        {
                            background = Texture2D.grayTexture,
                            textColor = Color.white
                        },
                        margin = new RectOffset(5, 5, 5, 5),
                        alignment = TextAnchor.MiddleCenter
                    }))
                {
                    DeveloperAuthCache.Delete();
                }
            }

            applicationManagementView.Render();
        }
    }
}