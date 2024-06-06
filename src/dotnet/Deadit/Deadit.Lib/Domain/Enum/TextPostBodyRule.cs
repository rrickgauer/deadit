using System.Text.Json.Serialization;

namespace Deadit.Lib.Domain.Enum;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TextPostBodyRule : ushort
{
    Optional = 1,
    Required = 2,
    NotAllowed = 3,
}



public static class TextPostBodyRuleExtensions
{
    public static string GetName(this TextPostBodyRule textPostBodyRule)
    {
        return System.Enum.GetName(textPostBodyRule)!;
    }
}