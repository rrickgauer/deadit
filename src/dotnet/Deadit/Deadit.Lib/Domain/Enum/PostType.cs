using Deadit.Lib.Domain.Attributes;
using Microsoft.AspNetCore.Html;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Web;

namespace Deadit.Lib.Domain.Enum;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PostType : ushort
{
    [EnumIcon(@"<i class=""bx bx-detail""></i>")]
    Text = 1,

    [EnumIcon(@"<i class=""bx bx-link""></i>")]
    Link = 2,
}


public static class PostTypeExtensions
{
    public static HtmlString GetIconHtml(this PostType postType)
    {
        return new(postType.GetIcon());
    }

    public static string GetIcon(this PostType postType)
    {
        string enumName = System.Enum.GetName(postType)!;

        if (typeof(PostType).GetField(enumName)?.GetCustomAttribute<EnumIconAttribute>() is not EnumIconAttribute attr)
        {
            throw new NotImplementedException();
        }

        return attr.Icon;
    }
}

