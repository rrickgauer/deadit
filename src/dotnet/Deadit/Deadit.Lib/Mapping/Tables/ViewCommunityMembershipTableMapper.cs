using Deadit.Lib.Domain.TableView;
using System.Data;

namespace Deadit.Lib.Mapping.Tables;

public class ViewCommunityMembershipTableMapper : TableMapper<ViewCommunityMembership>
{
    public override ViewCommunityMembership ToModel(DataRow row)
    {
        ViewCommunityMembership community = new();

        community.CommunityCreatedOn    = row.Field<DateTime>(GetColumnName(nameof(ViewCommunityMembership.CommunityCreatedOn)));
        community.CommunityDescription  = row.Field<string?>(GetColumnName(nameof(ViewCommunityMembership.CommunityDescription)));
        community.CommunityId           = row.Field<uint?>(GetColumnName(nameof(ViewCommunityMembership.CommunityId)));
        community.CommunityName         = row.Field<string?>(GetColumnName(nameof(ViewCommunityMembership.CommunityName)));
        community.CommunityOwnerId      = row.Field<uint?>(GetColumnName(nameof(ViewCommunityMembership.CommunityOwnerId)));
        community.CommunityTitle        = row.Field<string?>(GetColumnName(nameof(ViewCommunityMembership.CommunityTitle)));
        community.UserJoinedOn          = row.Field<DateTime>(GetColumnName(nameof(ViewCommunityMembership.UserJoinedOn)));
        community.CommunityCountMembers = row.Field<long>(GetColumnName(nameof(ViewCommunityMembership.CommunityCountMembers)));
        community.UserCreatedOn         = row.Field<DateTime>(GetColumnName(nameof(ViewCommunityMembership.UserCreatedOn)));
        community.UserEmail             = row.Field<string?>(GetColumnName(nameof(ViewCommunityMembership.UserEmail)));
        community.UserId                = row.Field<uint?>(GetColumnName(nameof(ViewCommunityMembership.UserId)));
        community.UserPassword          = row.Field<string?>(GetColumnName(nameof(ViewCommunityMembership.UserPassword)));
        community.UserUsername          = row.Field<string?>(GetColumnName(nameof(ViewCommunityMembership.UserUsername)));

        return community;
    }
}
