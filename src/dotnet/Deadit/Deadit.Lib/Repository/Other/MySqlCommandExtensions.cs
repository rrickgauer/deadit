using MySql.Data.MySqlClient;
using System.Data.Common;
using System.Data;
using Deadit.Lib.Domain.Paging;

namespace Deadit.Lib.Repository.Other;

public static class MySqlCommandExtensions
{
    public static async Task<DataTable> GetDataTableAsync(this MySqlCommand command)
    {
        DataTable dataTable = new();

        DbDataReader reader = await command.ExecuteReaderAsync();
        dataTable.Load(reader);

        return dataTable;
    }

    public static void AddPaginationParamters(this MySqlCommand command, Pagination pagination)
    {
        command.Parameters.AddWithValue("@pagination_limit", pagination.Limit);
        command.Parameters.AddWithValue("@pagination_offset", pagination.Offset);
    }
}
