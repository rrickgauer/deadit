using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.TableView;
using Deadit.Lib.Utility;
using System.Data;

namespace Deadit.Lib.Mapping.Tables;

public class ViewPostMapper : TableMapper<ViewPost>
{
    public override ViewPost ToModel(DataRow row)
    {
        ViewPost result = InheritanceUtility.GetParentProperties<ViewCommunity, ViewPost, ViewCommunityTableMapper>(row);

        result.PostId            = row.Field<Guid?>(GetColumnName(nameof(result.PostId)));
        result.PostCommunityId   = row.Field<uint?>(GetColumnName(nameof(result.PostCommunityId)));
        result.PostTitle         = row.Field<string?>(GetColumnName(nameof(result.PostTitle)));
        result.PostType          = (PostType?)row.Field<ushort?>(GetColumnName(nameof(result.PostType)));
        result.PostAuthorId      = row.Field<uint?>(GetColumnName(nameof(result.PostAuthorId)));
        result.PostCreatedOn     = row.Field<DateTime?>(GetColumnName(nameof(result.PostCreatedOn)));
        result.PostDeletedOn     = row.Field<DateTime?>(GetColumnName(nameof(result.PostDeletedOn)));
        result.PostArchivedOn    = row.Field<DateTime?>(GetColumnName(nameof(result.PostArchivedOn)));
        result.PostModRemovedOn  = row.Field<DateTime?>(GetColumnName(nameof(result.PostModRemovedOn)));
        result.PostCountComments = Convert.ToUInt32(row.Field<object?>(GetColumnName(nameof(result.PostCountComments))));

        row.SetVotingValues(result);

        return result;
    }
}

