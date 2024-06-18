using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Utility;
using System.Text.Json.Serialization;

namespace Deadit.Lib.Domain.Enum;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TextPostBodyRule : ushort
{
    [EnumDropdownDisplay("Text body is optional for all post types")]
    Optional = 1,

    [EnumDropdownDisplay("Text body is required for all text-only posts")]
    Required = 2,

    [EnumDropdownDisplay("Text body is not allowed")]
    NotAllowed = 3,
}



public static class TextPostBodyRuleExtensions
{
    public static string GetName(this TextPostBodyRule textPostBodyRule)
    {
        return System.Enum.GetName(textPostBodyRule)!;
    }


    public static string GetDisplay(this TextPostBodyRule textPostBodyRule)
    {
        return AttributeUtility.GetEnumAttribute<TextPostBodyRule, EnumDropdownDisplayAttribute>(textPostBodyRule)!.DisplayText;
    }
}