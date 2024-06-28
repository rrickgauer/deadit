using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Utility;
using System.Text.Json.Serialization;

namespace Deadit.Lib.Domain.Enum;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum FlairPostRule : ushort
{
    [EnumDropdownDisplay("Optional")]
    Optional = 1,

    [EnumDropdownDisplay("Required")]
    Required = 2,

    [EnumDropdownDisplay("Not allowed")]
    NotAllowed = 3,
}

public static class FlairPostRuleExtensions
{
    public static string GetName(this FlairPostRule rule)
    {
        string name = System.Enum.GetName(rule)!;
        return name;
    }

    public static string GetDropdownDisplay(this FlairPostRule rule)
    {
        var attr = AttributeUtility.GetEnumAttribute<FlairPostRule, EnumDropdownDisplayAttribute>(rule);
        return attr.DisplayText;
    }
}