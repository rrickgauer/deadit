using Deadit.Lib.Domain.TableView;
using System.Data;

namespace Deadit.Lib.Mapping.Tables;

public class ViewUserMapper : TableMapper<ViewUser>
{
    public override ViewUser ToModel(DataRow row)
    {
        ViewUser user = new()
        {
            UserId        = (uint?)row.Field<int?>(GetColumnName(nameof(ViewUser.UserId))),
            UserEmail     = row.Field<string?>(GetColumnName(nameof(ViewUser.UserEmail))),
            UserUsername  = row.Field<string?>(GetColumnName(nameof(ViewUser.UserUsername))),
            UserPassword  = row.Field<string?>(GetColumnName(nameof(ViewUser.UserPassword))),
            UserCreatedOn = row.Field<DateTime>(GetColumnName(nameof(ViewUser.UserCreatedOn))),
        };

        return user;
    }
}
