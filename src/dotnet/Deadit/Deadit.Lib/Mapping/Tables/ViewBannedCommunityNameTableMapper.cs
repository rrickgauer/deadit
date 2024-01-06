using Deadit.Lib.Domain.TableView;
using System.Data;

namespace Deadit.Lib.Mapping.Tables;

public class ViewBannedCommunityNameTableMapper : TableMapper<ViewBannedCommunityName>
{
    public override ViewBannedCommunityName ToModel(DataRow row)
    {
        ViewBannedCommunityName result = new()
        {
            Id = uint.Parse(row.Field<int?>(GetColumnName(nameof(ViewBannedCommunityName.Id))).ToString()),
            Name = row.Field<string?>(GetColumnName(nameof(ViewBannedCommunityName.Name)))
        };

        return result;
    }
}
