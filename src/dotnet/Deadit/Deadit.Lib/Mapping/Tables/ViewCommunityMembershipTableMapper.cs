using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.TableView;
using System.Data;

namespace Deadit.Lib.Mapping.Tables;

public class ViewCommunityMembershipTableMapper : TableMapper<ViewCommunityMembership>
{
    public override ViewCommunityMembership ToModel(DataRow row)
    {
        ViewCommunityMembership result = new()
        {
            CommunityCreatedOn          = row.Field<DateTime>(GetColumnName(nameof(ViewCommunityMembership.CommunityCreatedOn))),
            CommunityDescription        = row.Field<string?>(GetColumnName(nameof(ViewCommunityMembership.CommunityDescription))),
            CommunityId                 = row.Field<uint?>(GetColumnName(nameof(ViewCommunityMembership.CommunityId))),
            CommunityName               = row.Field<string?>(GetColumnName(nameof(ViewCommunityMembership.CommunityName))),
            CommunityOwnerId            = row.Field<uint?>(GetColumnName(nameof(ViewCommunityMembership.CommunityOwnerId))),
            CommunityTitle              = row.Field<string?>(GetColumnName(nameof(ViewCommunityMembership.CommunityTitle))),
            CommunityType               = (CommunityType)row.Field<ushort>(GetColumnName(nameof(ViewCommunityMembership.CommunityType))),
            CommunityTextPostBodyRule   = (TextPostBodyRule)row.Field<ushort>(GetColumnName(nameof(ViewCommunityMembership.CommunityTextPostBodyRule))),
            CommunityMembershipClosedOn = row.Field<DateTime?>(GetColumnName(nameof(ViewCommunityMembership.CommunityMembershipClosedOn))),
            UserJoinedOn                = row.Field<DateTime>(GetColumnName(nameof(ViewCommunityMembership.UserJoinedOn))),
            CommunityCountMembers       = row.Field<long>(GetColumnName(nameof(ViewCommunityMembership.CommunityCountMembers))),
            UserCreatedOn               = row.Field<DateTime>(GetColumnName(nameof(ViewCommunityMembership.UserCreatedOn))),
            UserEmail                   = row.Field<string?>(GetColumnName(nameof(ViewCommunityMembership.UserEmail))),
            UserId                      = row.Field<uint?>(GetColumnName(nameof(ViewCommunityMembership.UserId))),
            UserPassword                = row.Field<string?>(GetColumnName(nameof(ViewCommunityMembership.UserPassword))),
            UserUsername                = row.Field<string?>(GetColumnName(nameof(ViewCommunityMembership.UserUsername))),
        };

        return result;
    }
}
