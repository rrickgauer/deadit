using Deadit.Lib.Domain.TableView;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadit.Lib.Mapping.Tables;

public class ViewUserMapper : TableMapper<ViewUser>
{
    public override ViewUser ToModel(DataRow row)
    {

        int x = 120;

        ViewUser user = new()
        {
            Id        = row.Field<int?>(GetColumnName(nameof(ViewUser.Id))),
            Email     = row.Field<string?>(GetColumnName(nameof(ViewUser.Email))),
            Username  = row.Field<string?>(GetColumnName(nameof(ViewUser.Username))),
            Password  = row.Field<string?>(GetColumnName(nameof(ViewUser.Password))),
            CreatedOn = row.Field<DateTime>(GetColumnName(nameof(ViewUser.CreatedOn)))
        };

        return user;
    }
}
