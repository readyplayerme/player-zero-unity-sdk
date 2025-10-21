using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;

namespace PlayerZero.Api
{
    /// <summary>
    /// Provides utility methods for building URL query strings from objects.
    /// </summary>
    public static class QueryBuilder
    {
        /// <summary>
        /// Builds a URL query string from the public properties of the given object.
        /// Only non-null properties are included. Supports dictionaries and string arrays.
        /// </summary>
        /// <param name="queryParams">An object containing query parameters as properties.</param>
        public static string BuildQueryString(object queryParams)
        {
            var properties = queryParams.GetType().GetProperties()
                .Where(prop => prop.GetValue(queryParams, null) != null)
                .ToDictionary(
                    GetPropertyName,
                    prop => prop.GetValue(queryParams, null));

            if (properties.Count == 0)
                return string.Empty;

            var queryString = new StringBuilder();
            queryString.Append('?');

            foreach (var kvp in properties)
            {
                var key = kvp.Key;
                var value = kvp.Value;

                if (value is IDictionary dictionary)
                {
                    foreach (DictionaryEntry entry in dictionary)
                    {
                        queryString.Append(
                            $"{Uri.EscapeDataString(entry.Key.ToString())}={Uri.EscapeDataString(entry.Value.ToString())}&");
                    }
                }
                else if (value is IEnumerable<string> stringArray)
                {
                    foreach (var item in stringArray)
                    {
                        queryString.Append($"{Uri.EscapeDataString(key)}={Uri.EscapeDataString(item)}&");
                    }
                }
                else
                {
                    queryString.Append($"{Uri.EscapeDataString(key)}={Uri.EscapeDataString(value.ToString())}&");
                }
            }
            
            // Remove the trailing '&' and return the query string
            return queryString.ToString().TrimEnd('&');
        }
        
        /// <summary>
        /// Gets the property name for query string construction.
        /// Uses the JsonProperty attribute if present; otherwise, uses the property name.
        /// </summary>
        /// <param name="prop">The property info.</param>
        /// <returns>The property name to use in the query string.</returns>
        private static string GetPropertyName(MemberInfo prop)
        {
            return prop.GetCustomAttribute<JsonPropertyAttribute>() != null
                ? prop.GetCustomAttribute<JsonPropertyAttribute>().PropertyName
                : prop.Name;
        }
    }
    
    
}