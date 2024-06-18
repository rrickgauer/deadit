using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Utility;
using Microsoft.AspNetCore.Html;
using System.Text.Json.Serialization;

namespace Deadit.Lib.Domain.Enum;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CommunityType : ushort
{
    [EnumDropdownDisplay(@"<span class=""fw-medium"">Private: </span> <span class=""opacity-75"">Only approved users can view this community</span>")]
    Private = 1,

    [EnumDropdownDisplay(@"<span class=""fw-medium"">Public:</span> <span class=""opacity-75"">Anyone can view community posts</span>")]
    Public = 2,
}

public static class CommunityTypeExtensions
{
    public static string GetName(this CommunityType communityType)
    {
        return System.Enum.GetName(communityType)!;
    }

    public static HtmlString GetDisplay(this CommunityType communityType)
    {
        var text = AttributeUtility.GetEnumAttribute<CommunityType, EnumDropdownDisplayAttribute>(communityType).DisplayText;

        return HtmlUtility.ToHtml(text);
    }
}
