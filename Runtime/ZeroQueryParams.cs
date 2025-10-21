using System;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerZero.Runtime.DeepLinking

{
    /// <summary>
    /// Provides methods for extracting and selecting query parameters from URLs for deep linking.
    /// </summary>
    public static class ZeroQueryParams
    {
        /// <summary>
        /// Stores parsed query parameters.
        /// </summary>
        private static Dictionary<string, string> _queryParameters = new Dictionary<string, string>();

        /// <summary>
        /// Gets all query parameters from the current application URL or desktop deep link (Windows).
        /// </summary>
        /// <returns>A dictionary of all query parameters.</returns>
        public static Dictionary<string, string> GetParams()
        {
#if UNITY_STANDALONE_WIN && !UNITY_EDITOR
            return GetParams(DeepLinkHandler.GetDesktopUrl());
#endif
            return GetParams(Application.absoluteURL);
        }
        
        /// <summary>
        /// Returns only the specified query parameters, logging a warning for missing keys.
        /// </summary>
        /// <param name="keys">The keys to select from the query parameters.</param>
        /// <returns>A dictionary of selected query parameters.</returns>
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
        /// <param name="url">The URL to parse.</param>
        /// <returns>A dictionary of parsed query parameters.</returns>
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