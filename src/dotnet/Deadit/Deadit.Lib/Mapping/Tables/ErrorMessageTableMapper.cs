using Deadit.Lib.Domain.Model;
using System.Data;

namespace Deadit.Lib.Mapping.Tables;

public class ErrorMessageTableMapper : TableMapper<ErrorMessage>
{
    public override ErrorMessage ToModel(DataRow row)
    {
        ErrorMessage errorMessage = new()
        {
            Id = row.Field<int?>(GetColumnName(nameof(ErrorMessage.Id))),
            Message = row.Field<string?>(GetColumnName(nameof(ErrorMessage.Message)))
        };

        return errorMessage;
    }
}
