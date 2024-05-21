using Microsoft.AspNetCore.Html;

namespace Deadit.Lib.Utility;

public class HtmlUtility
{
    public static HtmlString ToHtml(object? data)
    {
        return new HtmlString($"{data}");
    }
}
