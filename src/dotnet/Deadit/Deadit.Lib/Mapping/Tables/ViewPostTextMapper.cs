using Deadit.Lib.Domain.TableView;
using Deadit.Lib.Utility;
using System.Data;

namespace Deadit.Lib.Mapping.Tables;

public class ViewPostTextMapper : TableMapper<ViewPostText>
{
    public override ViewPostText ToModel(DataRow row)
    {
        ViewPostText result = InheritanceUtility.GetParentProperties<ViewPost, ViewPostText, ViewPostMapper>(row);

        result.PostContent = row.Field<string?>(GetColumnName(nameof(result.PostContent)));

        return result;
    }
}
