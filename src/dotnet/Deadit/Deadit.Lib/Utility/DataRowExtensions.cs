using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Contracts;
using System.Data;
using System.Reflection;

namespace Deadit.Lib.Utility;

public static class DataRowExtensions
{
    public static void SetVotingValues(this DataRow dataRow, IVoteScore voteable)
    {
        var propertyNames = IVoteScore.PropertyNames;

        var properties = voteable.GetType().GetProperties().Where(p => propertyNames.Contains(p.Name));


        foreach (var property in properties)
        {
            var columnName = property.GetCustomAttribute<SqlColumnAttribute>()?.ColumnName ?? property.Name;

            var value = dataRow.Field<long?>(columnName);

            if (value.HasValue)
            {
                property.SetValue(voteable, value.Value);
            }
        }
    }
}
