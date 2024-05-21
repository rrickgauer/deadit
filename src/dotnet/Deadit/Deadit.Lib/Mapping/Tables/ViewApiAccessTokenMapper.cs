using Deadit.Lib.Domain.TableView;
using Deadit.Lib.Utility;
using System.Data;

namespace Deadit.Lib.Mapping.Tables;

public class ViewApiAccessTokenMapper : TableMapper<ViewApiAccessToken>
{
    public override ViewApiAccessToken ToModel(DataRow row)
    {
        ViewApiAccessToken result = InheritanceUtility.GetParentProperties<ViewUser, ViewApiAccessToken, ViewUserMapper>(row);

        result.TokenId = Convert.ToUInt32(row.Field<object?>(GetColumnName(nameof(result.TokenId))));
        result.Token = row.Field<Guid?>(GetColumnName(nameof(result.Token)));
        result.TokenUserId = row.Field<uint?>(GetColumnName(nameof(result.TokenUserId)));
        result.TokenCreatedOn = row.Field<DateTime?>(GetColumnName(nameof(result.TokenCreatedOn)));

        return result;
    }
}
