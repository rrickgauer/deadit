namespace Deadit.Lib.Domain.Enum;

public enum SortDirection
{
    Ascending,
    Descending,
}

public static class SortDirectionExtensions
{
    public static string GetSqlValue(this SortDirection direction)
    {
        return direction switch
        {
            SortDirection.Ascending  => "ASC",
            SortDirection.Descending => "DESC",
            _                        => throw new NotImplementedException(),
        };
    }
}
