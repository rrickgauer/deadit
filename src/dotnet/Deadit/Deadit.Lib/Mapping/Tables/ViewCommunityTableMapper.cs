using Deadit.Lib.Domain.TableView;
using System.Data;

namespace Deadit.Lib.Mapping.Tables;

public class ViewCommunityTableMapper : TableMapper<ViewCommunity>
{
    public override ViewCommunity ToModel(DataRow row)
    {
        ViewCommunity result = new()
        {
            CommunityId           = Convert.ToUInt32((row.Field<object?>(GetColumnName(nameof(ViewCommunity.CommunityId))))),
            CommunityName         = row.Field<string?>(GetColumnName(nameof(ViewCommunity.CommunityName))),
            CommunityTitle        = row.Field<string?>(GetColumnName(nameof(ViewCommunity.CommunityTitle))),
            CommunityOwnerId      = Convert.ToUInt32(row.Field<object?>(GetColumnName(nameof(ViewCommunity.CommunityOwnerId)))),
            CommunityDescription  = row.Field<string?>(GetColumnName(nameof(ViewCommunity.CommunityDescription))),
            CommunityCreatedOn    = row.Field<DateTime>(GetColumnName(nameof(ViewCommunity.CommunityCreatedOn))),
            CommunityCountMembers = row.Field<long>(GetColumnName(nameof(ViewCommunity.CommunityCountMembers))),
        };


        return result;
    }
}
