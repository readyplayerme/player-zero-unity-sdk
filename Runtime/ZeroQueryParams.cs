using System;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerZero.Runtime.DeepLinking
{
    public static class ZeroQueryParams
    {
        private static Dictionary<string, string> _queryParameters = new Dictionary<string, string>();

        public static Dictionary<string, string> GetParams()
        {
#if UNITY_STANDALONE_WIN && !UNITY_EDITOR
            return GetParams(DeepLinkHandler.GetDesktopUrl());
#endif
            return GetParams(Application.absoluteURL);
        }
        
        /// <summary>
        /// Returns only the specified query parameters.
        /// Logs a warning for any requested keys that are not present.
        /// </summary>
        public static Dictionary<string, string> GetSelectedParams(params string[] keys)
        {
            var allParams = GetParams(Application.absoluteURL);
            var selectedParams = new Dictionary<string, string>();

            foreach (var key in keys)
            {
                if (allParams.TryGetValue(key, out var value))
                {
                    selectedParams[key] = value;
                }
                else
                {
                    Debug.LogWarning($"Query parameter '{key}' not found in URL.");
                }
            }

            return selectedParams;
        }

        /// <summary>
        /// Parses query parameters from the given URL.
        /// </summary>
        public static Dictionary<string, string> GetParams(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return _queryParameters;
            }

            var uri = new Uri(url);
            var query = uri.Query;
            
            if (string.IsNullOrEmpty(query))
                return _queryParameters;

            var queryParamsArray = query.TrimStart('?').Split('&');

            foreach (var param in queryParamsArray)
            {
                var keyValue = param.Split('=');
                if (keyValue.Length != 2) continue;

                var key = Uri.UnescapeDataString(keyValue[0]);
                var value = Uri.UnescapeDataString(keyValue[1]);

                _queryParameters[key] = value;
            }

            return _queryParameters;
        }
    }
}