using Deadit.Lib.Domain.Enum;

namespace Deadit.Lib.Domain.Attributes;



[AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
public class SqlSortAttribute<T>(string columnName, SortDirection sortDirection=SortDirection.Ascending) : Attribute
{
    public string ColumnName { get; } = columnName;
    public SortDirection SortDirection { get; } = sortDirection;

    public string Clause => $"{ColumnName} {SortDirection.GetSqlValue()}";

    public Type ModelType => typeof(T);
}
