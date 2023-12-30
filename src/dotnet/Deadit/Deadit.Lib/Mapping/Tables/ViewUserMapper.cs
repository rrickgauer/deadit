using Deadit.Lib.Domain.TableView;
using System.Data;

namespace Deadit.Lib.Mapping.Tables;

public class ViewUserMapper : TableMapper<ViewUser>
{
    public override ViewUser ToModel(DataRow row)
    {
        ViewUser user = new()
        {
            Id        = (uint?)row.Field<int?>(GetColumnName(nameof(ViewUser.Id))),
            Email     = row.Field<string?>(GetColumnName(nameof(ViewUser.Email))),
            Username  = row.Field<string?>(GetColumnName(nameof(ViewUser.Username))),
            Password  = row.Field<string?>(GetColumnName(nameof(ViewUser.Password))),
            CreatedOn = row.Field<DateTime>(GetColumnName(nameof(ViewUser.CreatedOn))),
        };

        return user;
    }
}
