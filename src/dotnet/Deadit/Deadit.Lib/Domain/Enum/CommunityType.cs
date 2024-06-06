using System.Text.Json.Serialization;

namespace Deadit.Lib.Domain.Enum;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CommunityType : ushort
{
    Private = 1,
    Public = 2,
}

public static class CommunityTypeExtensions
{
    public static string GetName(this CommunityType communityType)
    {
        return System.Enum.GetName(communityType)!;
    }
}
