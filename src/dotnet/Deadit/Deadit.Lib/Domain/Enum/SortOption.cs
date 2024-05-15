using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.TableView;
using System.Reflection;
using System.Text.Json.Serialization;

namespace Deadit.Lib.Domain.Enum;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SortOption
{
    [SqlSort<ViewComment>("comment_created_on", SortDirection.Descending)]
    New,

    [SqlSort<ViewComment>("comment_created_on", SortDirection.Ascending)]
    Old,

    [SqlSort<ViewComment>("comment_count_votes_score", SortDirection.Descending)]
    Score,
}


public static class SortOptionExtensions
{
    private const string OptionHtmlTemplate = @"<option class=""items-sort-option"" value=""{0}"" {1}>{0}</option>";

    /// <summary>
    /// Build the html string <select> elements for the SortOptions.
    /// Sets the specified one to selected.
    /// </summary>
    /// <param name="sortOption"></param>
    /// <returns></returns>
    public static string GetSortOptionHtml(this SortOption sortOption)
    {
        var allOptions = System.Enum.GetNames<SortOption>().ToList();

        string html = string.Empty;

        foreach(var option in allOptions)
        {
            var optionValue = System.Enum.Parse<SortOption>(option);

            var selected = optionValue == sortOption ? "selected" : string.Empty;

            html += string.Format(OptionHtmlTemplate, option, selected);
        }

        return html;
    }

    public static string GetOrderByClause<T>(this SortOption sortOption)
    {
        var enumField = typeof(SortOption).GetField(System.Enum.GetName(sortOption)!);

        if (enumField?.GetCustomAttribute<SqlSortAttribute<T>>() is not SqlSortAttribute<T> attr)
        {
            throw new NotImplementedException();
        }

        return attr.Clause;
    }
}
