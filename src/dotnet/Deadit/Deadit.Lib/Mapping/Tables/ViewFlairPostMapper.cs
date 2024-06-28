using Deadit.Lib.Domain.TableView;
using Deadit.Lib.Utility;
using System.Data;

namespace Deadit.Lib.Mapping.Tables;

public class ViewFlairPostMapper : TableMapper<ViewFlairPost>
{
    public override ViewFlairPost ToModel(DataRow row)
    {
        ViewFlairPost result = InheritanceUtility.GetParentProperties<ViewCommunity, ViewFlairPost, ViewCommunityTableMapper>(row);

        result.FlairPostId          = Convert.ToUInt32(row.Field<object?>(GetColumnName(nameof(result.FlairPostId))));
        result.FlairPostCommunityId = row.Field<uint?>(GetColumnName(nameof(result.FlairPostCommunityId)));
        result.FlairPostName        = row.Field<string?>(GetColumnName(nameof(result.FlairPostName)));
        result.FlairPostColor       = row.Field<string?>(GetColumnName(nameof(result.FlairPostColor)));
        result.FlairPostCreatedOn   = row.Field<DateTime>(GetColumnName(nameof(result.FlairPostCreatedOn)));

        return result;
    }
}
