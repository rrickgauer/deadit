using System.Runtime.CompilerServices;

namespace Deadit.Lib.Domain.Attributes;

[AttributeUsage(AttributeTargets.Property, Inherited = true)]
public class SqlColumnAttribute([CallerMemberName] string columnName = "") : Attribute
{
    public string ColumnName { get; } = columnName;
}