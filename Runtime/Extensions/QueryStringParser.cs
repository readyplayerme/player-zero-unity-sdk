using System;
using System.Collections.Generic;

public static class QueryStringParser
{
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