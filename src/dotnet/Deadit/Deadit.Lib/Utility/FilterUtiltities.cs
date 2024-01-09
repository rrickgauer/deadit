using Microsoft.AspNetCore.Mvc.Filters;

namespace Deadit.Lib.Utility;

public static class FilterUtiltities
{

    public static string GetCommunityNameRouteValue(ActionExecutingContext context) => GetRequestRouteValue<string>(context, "communityName");


    /// <summary>
    /// Get the specified request value with the matching key.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="context"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static T GetRequestRouteValue<T>(ActionExecutingContext context, string key)
    {
        var value = (T)context.ActionArguments[key];

        return value;
    }
}
