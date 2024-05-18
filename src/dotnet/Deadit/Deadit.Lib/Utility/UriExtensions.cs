using System.Web;

namespace Deadit.Lib.Utility;

public static class UriExtensions
{
    public static string SetQueryParmValue(this Uri uri, string key, object? value)
    {
        string baseUri = uri.GetLeftPart(UriPartial.Path);

        var parsedQuery = HttpUtility.ParseQueryString(uri.Query);

        if (value == null)
        {
            parsedQuery.Remove(key);
        }
        else
        {
            parsedQuery.Set(key, $"{value}");
        }

        UriBuilder builder = new(baseUri)
        {
            Query = parsedQuery.ToString(),
        };

        return builder.ToString();
    }

    public static string SetQueryParmValue(string uri, string key, object? value)
    {
        Uri url = new(uri);

        return url.SetQueryParmValue(key, value);
    }
}

