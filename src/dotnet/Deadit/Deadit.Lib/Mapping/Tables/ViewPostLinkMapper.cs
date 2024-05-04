using Deadit.Lib.Domain.TableView;
using Deadit.Lib.Utility;
using System.Data;

namespace Deadit.Lib.Mapping.Tables;

public class ViewPostLinkMapper : TableMapper<ViewPostLink>
{
    public override ViewPostLink ToModel(DataRow row)
    {
        ViewPostLink result = InheritanceUtility.GetParentProperties<ViewPost, ViewPostLink, ViewPostTableMapper>(row);

        result.PostUrl = row.Field<string?>(GetColumnName(nameof(result.PostUrl)));

        return result;
    }
}
