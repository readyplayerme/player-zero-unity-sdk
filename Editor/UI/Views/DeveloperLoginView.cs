using System;
using PlayerZero.Editor.UI.ViewModels;
using UnityEditor;
using UnityEngine;

namespace PlayerZero.Editor.UI.Views
{
    public class DeveloperLoginView
    {
        private readonly DeveloperLoginViewModel viewModel;

        public DeveloperLoginView(DeveloperLoginViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public void Render(Action onLogin)
        {
            using (new GUILayout.VerticalScope(new GUIStyle() { margin = new RectOffset(9, 9, 9, 0), }))
            {
                EditorGUILayout.LabelField("Sign in with your Player Zero developer account to start.");

                EditorGUILayout.Space(10);

                viewModel.Username = EditorGUILayout.TextField("Username:", viewModel.Username);

                EditorGUILayout.Space(5);

                viewModel.Password = EditorGUILayout.PasswordField("Password:", viewModel.Password);

                EditorGUILayout.Space(5);
            }

            if (GUILayout.Button(viewModel.Loading ? "Loading..." : "Sign In", new GUIStyle(GUI.skin.button)
                {
                    margin = new RectOffset(12, 12, 0, 0)
                }))
            {

#pragma warning disable CS4014
                viewModel.SignIn(onSuccess: onLogin);
#pragma warning restore CS4014
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
                        left = 12, 
                    }
                });
            } 
        }
    }
}