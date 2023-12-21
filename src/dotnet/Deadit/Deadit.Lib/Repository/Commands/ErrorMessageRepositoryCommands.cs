using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadit.Lib.Repository.Commands;

public sealed class ErrorMessageRepositoryCommands
{
    public const string SelectAll = @"
        SELECT
            e.id AS id,
            e.message AS message
        FROM
            Error_Message e
        ORDER BY
            id ASC;";
}
