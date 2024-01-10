using Deadit.Lib.Domain.TableView;
using System.Data;

namespace Deadit.Lib.Mapping.Tables;

public class ViewCommunityTableMapper : TableMapper<ViewCommunity>
{
    public override ViewCommunity ToModel(DataRow row)
    {
        ViewCommunity result = new();

        result.CommunityId = Convert.ToUInt32((row.Field<object?>(GetColumnName(nameof(ViewCommunity.CommunityId)))));
        result.CommunityName = row.Field<string?>(GetColumnName(nameof(ViewCommunity.CommunityName)));
        result.CommunityTitle = row.Field<string?>(GetColumnName(nameof(ViewCommunity.CommunityTitle)));
        result.CommunityOwnerId = Convert.ToUInt32(row.Field<object?>(GetColumnName(nameof(ViewCommunity.CommunityOwnerId))));
        result.CommunityDescription = row.Field<string?>(GetColumnName(nameof(ViewCommunity.CommunityDescription)));
        result.CommunityCreatedOn = row.Field<DateTime>(GetColumnName(nameof(ViewCommunity.CommunityCreatedOn)));
        result.CommunityCountMembers = row.Field<long>(GetColumnName(nameof(ViewCommunity.CommunityCountMembers)));
        

        return result;
    }
}
