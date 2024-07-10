using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.TableView;
using System.Data;

namespace Deadit.Lib.Mapping.Tables;

public class ViewCommunityTableMapper : TableMapper<ViewCommunity>
{
    public override ViewCommunity ToModel(DataRow row)
    {
        ViewCommunity result = new()
        {
            CommunityId                 = Convert.ToUInt32((row.Field<object?>(GetColumnName(nameof(result.CommunityId))))),
            CommunityName               = row.Field<string?>(GetColumnName(nameof(result.CommunityName))),
            CommunityTitle              = row.Field<string?>(GetColumnName(nameof(result.CommunityTitle))),
            CommunityOwnerId            = Convert.ToUInt32(row.Field<object?>(GetColumnName(nameof(result.CommunityOwnerId)))),
            CommunityDescription        = row.Field<string?>(GetColumnName(nameof(result.CommunityDescription))),
            CommunityCreatedOn          = row.Field<DateTime>(GetColumnName(nameof(result.CommunityCreatedOn))),
            CommunityCountMembers       = row.Field<long>(GetColumnName(nameof(result.CommunityCountMembers))),
            CommunityType               = (CommunityType)row.Field<ushort>(GetColumnName(nameof(result.CommunityType))),
            CommunityTextPostBodyRule   = (TextPostBodyRule)row.Field<ushort>(GetColumnName(nameof(result.CommunityTextPostBodyRule))),
            CommunityMembershipClosedOn = row.Field<DateTime?>(GetColumnName(nameof(result.CommunityMembershipClosedOn))),
            CommunityFlairPostRule      = (FlairPostRule)row.Field<ushort>(GetColumnName(nameof(result.CommunityFlairPostRule))),
        };

        return result;
    }
}
