using Deadit.Lib.Utility;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Deadit.Lib.Domain.Enum;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CommunityMembersSortField
{
    [Description("user_joined_community_on")]
    JoinedOn,

    [Description("user_username")]
    Username,
}


public static class CommunityMembersSortTypeExtensions
{
    public static string GetSqlColumn(this CommunityMembersSortField sortType)
    {
        var attr = AttributeUtility.GetEnumAttribute<CommunityMembersSortField, DescriptionAttribute>(sortType);

        return attr.Description;
    }
}