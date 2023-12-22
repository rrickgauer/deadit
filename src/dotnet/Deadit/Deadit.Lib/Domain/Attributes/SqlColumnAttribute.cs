using System.Runtime.CompilerServices;

namespace Deadit.Lib.Domain.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class SqlColumnAttribute : Attribute
{
    public string ColumnName { get; }

    public SqlColumnAttribute([CallerMemberName] string columnName = "")
    {
        ColumnName = columnName;
    }
}