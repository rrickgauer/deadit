using Deadit.Lib.Domain.TableView;
using System.Data;

namespace Deadit.Lib.Mapping.Tables;

public class ViewCommunityTableMapper : TableMapper<ViewCommunity>
{
    public override ViewCommunity ToModel(DataRow row)
    {
        ViewCommunity result = new();

        result.Id = Convert.ToUInt32((row.Field<object?>(GetColumnName(nameof(ViewCommunity.Id)))));
        result.Name = row.Field<string?>(GetColumnName(nameof(ViewCommunity.Name)));
        result.Title = row.Field<string?>(GetColumnName(nameof(ViewCommunity.Title)));
        result.OwnerId = Convert.ToInt32(row.Field<object?>(GetColumnName(nameof(ViewCommunity.OwnerId))));
        result.Description = row.Field<string?>(GetColumnName(nameof(ViewCommunity.Description)));
        result.CreatedOn = row.Field<DateTime>(GetColumnName(nameof(ViewCommunity.CreatedOn)));
        result.CountMembers = row.Field<long>(GetColumnName(nameof(ViewCommunity.CountMembers)));
        

        return result;
    }
}
