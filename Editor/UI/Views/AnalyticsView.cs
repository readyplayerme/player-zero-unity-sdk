using System;
using System.Threading.Tasks;
using PlayerZero.Api.V1;
using PlayerZero.Data;
using UnityEditor;
using UnityEngine;

namespace PlayerZero.Editor.UI.Views
{
    public class AnalyticsView
    {
        private GUIStyle headerStyle;
        private GUIStyle statusStyle;
        private GameEventApi gameEventApi;

        public void Render(string gameId, string defaultAvatarId)
        {
            EnsureStylesInitialized();

            GUILayout.Label("Game Analytics", headerStyle);
            DrawStatus("Default Avatar ID", defaultAvatarId);
            DrawStatus("Game ID", gameId);
            EditorGUILayout.Space();

            GUI.enabled = !string.IsNullOrEmpty(gameId) && !string.IsNullOrEmpty(defaultAvatarId);
            var buttonStyle = new GUIStyle(GUI.skin.button)
            {
                margin = new RectOffset(10, 10, 0, 0)
            };
            if (GUILayout.Button("Send Test Event", buttonStyle, GUILayout.Height(30)))
            {
                SendTestAnalyticsEvent(gameId, defaultAvatarId);
            }
            GUI.enabled = true;
        }

        private void EnsureStylesInitialized()
        {
            if (headerStyle == null)
            {
                headerStyle = new GUIStyle(EditorStyles.boldLabel)
                {
                    fontSize = 14,
                    normal = new GUIStyleState { textColor = Color.white },
                    margin = new RectOffset(10, 10, 0, 10)
                };
            }

            if (statusStyle == null)
            {
                statusStyle = new GUIStyle(EditorStyles.label)
                {
                    wordWrap = true,
                    normal = new GUIStyleState { textColor = Color.white },
                    padding = new RectOffset(10, 10, 0, 0)
                };
            }
            gameEventApi ??= new GameEventApi();
        }

        private void DrawStatus(string label, string value)
        {
            bool isSet = !string.IsNullOrEmpty(value);
            GUIStyle labelStyle = new GUIStyle(EditorStyles.label)
            {
                normal = new GUIStyleState
                {
                    textColor = isSet ? Color.green : Color.red,
                    
                },
            };
            GUILayout.BeginHorizontal();
            GUILayout.Label($"{label}:", statusStyle, GUILayout.Width(150));
            GUILayout.Label(isSet ? "Set" : "Missing", labelStyle);
            GUILayout.EndHorizontal();

            if (!isSet)
            {
                var horizontalStyle = new GUIStyle(GUI.skin.label)
                {
                    margin = new RectOffset(10, 10, 0, 0)
                };
                GUILayout.BeginHorizontal(horizontalStyle);
                EditorGUILayout.HelpBox($"{label} is required for analytics to work.", MessageType.Warning);
                GUILayout.EndHorizontal();
            }
        }

        private async Task SendTestAnalyticsEvent(string gameId, string defaultAvatarId)
        {
            var settings = Resources.Load<Settings>("PlayerZeroSettings");
            var deviceContext = DeviceAnalytics.GetDeviceInfo();
            var properties = new AvatarSessionStartedProperties
            {
                GameId = gameId,
                GameSessionId = Guid.NewGuid().ToString(),
                AvatarId = defaultAvatarId,
                SdkVersion = settings.Version,
                SdkPlatform = "Unity",
                SessionId = Guid.NewGuid().ToString(),
                DeviceContext = deviceContext,
            };
            var gameEvent = new AvatarSessionStartedEvent
            {
                Properties = properties
            };
            
            var response = await gameEventApi.SendGameEventAsync<AvatarSessionStartedEvent, AvatarSessionStartedProperties>(gameEvent);
            if (response.IsSuccess)
            {
                EditorUtility.DisplayDialog("Test Event Success", "A Player Zero event sent successfully!", "OK");
            }
            else
            {
                EditorUtility.DisplayDialog("Test Event Failed", "A Player Zero event failed to send!", "OK");
            }
        }
    }
}
