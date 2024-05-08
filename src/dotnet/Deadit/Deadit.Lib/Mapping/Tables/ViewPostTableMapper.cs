using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.TableView;
using Deadit.Lib.Utility;
using System.Data;

namespace Deadit.Lib.Mapping.Tables;

public class ViewPostTableMapper : TableMapper<ViewPost>
{
    public override ViewPost ToModel(DataRow row)
    {
        ViewPost result = InheritanceUtility.GetParentProperties<ViewCommunity, ViewPost, ViewCommunityTableMapper>(row);

        result.PostId          = row.Field<Guid?>(GetColumnName(nameof(result.PostId)));
        result.PostCommunityId = row.Field<uint?>(GetColumnName(nameof(result.PostCommunityId)));
        result.PostTitle       = row.Field<string?>(GetColumnName(nameof(result.PostTitle)));
        result.PostType        = (PostType?)row.Field<ushort?>(GetColumnName(nameof(result.PostType)));
        result.PostAuthorId    = row.Field<uint?>(GetColumnName(nameof(result.PostAuthorId)));
        result.PostCreatedOn   = row.Field<DateTime?>(GetColumnName(nameof(result.PostCreatedOn)));

        return result;
    }
}

