using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlayerZero.Data;
using PlayerZero.Editor.Api.V1.Analytics;
using PlayerZero.Editor.Api.V1.Analytics.Models;
using PlayerZero.Editor.Api.V1.Auth;
using PlayerZero.Editor.Api.V1.Auth.Models;
using PlayerZero.Editor.Cache.EditorPrefs;
using UnityEditor;
using UnityEngine;

namespace PlayerZero.Editor.UI.ViewModels
{
    public class DeveloperLoginViewModel
    {

        private readonly DeveloperAuthApi _developerAuthApi;
        private readonly AnalyticsApi _analyticsApi;
        
        public string Username { get; set; }

        public string Password { get; set; }

        public bool Loading { get; private set; }

        public string Error { get; private set; }

        public DeveloperLoginViewModel(
            DeveloperAuthApi developerAuthApi,
            AnalyticsApi analyticsApi
        )
        {
            _developerAuthApi = developerAuthApi;
            _analyticsApi = analyticsApi;
        }

        public async Task SignIn(Action onSuccess)
        {
            Loading = true;

            var response = await _developerAuthApi.LoginAsync(new DeveloperLoginRequest()
            {
                Payload = new DeveloperLoginRequestBody
                {
                    Email = Username,
                    Password = Password
                }
            });

            var settings = Resources.Load<Settings>("ReadyPlayerMeSettings");

            EditorUtility.SetDirty(settings);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            if (!response.IsSuccess)
            {
                Error = "Studio login failed. Double check your username and password.";
                Loading = false;
                return;
            }

            DeveloperAuthCache.Data = new DeveloperAuth()
            {
                Name = response.Data.Name,
                Token = response.Data.Token,
                RefreshToken = response.Data.RefreshToken,
            };

            _analyticsApi.SendEvent(new AnalyticsEventRequest()
            {
                Payload = new AnalyticsEventRequestBody()
                {
                    Event = "next gen unity sdk action",
                    Properties =
                    {
                        { "type", "Sign In" },
                        { "username", Username }
                    }
                }
            });

            Loading = false;
            onSuccess();
        }
    }
}