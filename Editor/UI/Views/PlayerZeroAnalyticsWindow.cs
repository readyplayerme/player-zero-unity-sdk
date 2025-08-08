using UnityEditor;
using UnityEngine;

namespace PlayerZero.Editor.UI.Views
{
    public class AnalyticsView
    {
        private GUIStyle headerStyle;
        private GUIStyle statusStyle;

        public void Render(string gameId, string defaultAvatarId)
        {
            EnsureStylesInitialized();

            GUILayout.Label("Game Analytics", headerStyle);

            EditorGUILayout.Space();
            DrawStatus("Default Avatar ID", defaultAvatarId);
            DrawStatus("Game ID", gameId);
            EditorGUILayout.Space();

            GUI.enabled = !string.IsNullOrEmpty(gameId) && !string.IsNullOrEmpty(defaultAvatarId);
            if (GUILayout.Button("Send Test Event", GUILayout.Height(30)))
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
                    normal = new GUIStyleState { textColor = Color.white }
                };
            }
        }

        private void DrawStatus(string label, string value)
        {
            bool isSet = !string.IsNullOrEmpty(value);
            GUIStyle labelStyle = new GUIStyle(EditorStyles.label)
            {
                normal = new GUIStyleState
                {
                    textColor = isSet ? Color.green : Color.red
                }
            };

            GUILayout.BeginHorizontal();
            GUILayout.Label($"{label}:", statusStyle, GUILayout.Width(150));
            GUILayout.Label(isSet ? "Set" : "Missing", labelStyle);
            GUILayout.EndHorizontal();

            if (!isSet)
            {
                EditorGUILayout.HelpBox($"{label} is required for analytics to work.", MessageType.Warning);
            }
        }

        private void SendTestAnalyticsEvent(string gameId, string defaultAvatarId)
        {
            Debug.Log($"[PlayerZero] Sending test event...\nGame ID: {gameId}\nAvatar ID: {defaultAvatarId}");
            EditorUtility.DisplayDialog("Test Event", "Test event sent successfully!", "OK");
        }
    }
}
