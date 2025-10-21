using System;
using System.Collections.Generic;

namespace PlayerZero
{
    /// <summary>
    /// Utility methods for parsing query strings from URLs.
    /// </summary>
    public static class QueryStringParser
    {
        /// <summary>
        /// Parses the query string from the given URL into a dictionary of key-value pairs.
        /// Returns an empty dictionary if no query string is present.
        /// </summary>
        /// <param name="url">The URL containing the query string.</param>
        /// <returns>A dictionary of query parameters.</returns>
        public static Dictionary<string, string> Parse(string url)
        {
            var result = new Dictionary<string, string>();

            if (!url.Contains("?")) return result;
            var query = url.Substring(url.IndexOf('?') + 1);
            var pairs = query.Split('&');

            foreach (var pair in pairs)
            {
                var kv = pair.Split('=');
                if (kv.Length == 2)
                {
                    string key = Uri.UnescapeDataString(kv[0]);
                    string value = Uri.UnescapeDataString(kv[1]);
                    result[key] = value;
                }
            }

            return result;
        }
    }
}
