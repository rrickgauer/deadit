using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Utility;
using System.Text.Json.Serialization;

namespace Deadit.Lib.Domain.Enum;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TopPostSort
{
    [TimeSpanDays(1)]
    Day,

    [TimeSpanDays(7)]
    Week,

    [TimeSpanDays(31)]
    Month,

    [TimeSpanDays(365)]
    Year,

    [TimeSpanDays(int.MinValue)]
    All,
}



public static class TopPostSortExtensions
{
    private const string OptionHtmlTemplate = @"<option class=""top-post-sort-option"" value=""{0}"" {1}>{0}</option>";

    public static string GetSortOptionHtml(this TopPostSort sortOption)
    {
        var allOptions = System.Enum.GetNames<TopPostSort>().ToList();

        string html = string.Empty;

        foreach (var option in allOptions)
        {
            var optionValue = System.Enum.Parse<TopPostSort>(option);

            var selected = optionValue == sortOption ? "selected" : string.Empty;

            html += string.Format(OptionHtmlTemplate, option, selected);
        }

        return html;
    }

    public static DateTime GetStartingDate(this TopPostSort sort)
    {
        var attr = AttributeUtility.GetEnumAttribute<TopPostSort, TimeSpanDaysAttribute>(sort);

        if (attr.IsAllTime)
        {
            return DateTime.MinValue;
        }

        var timeSpan = TimeSpan.FromDays(attr.Days + 1);

        return DateTime.UtcNow.Subtract(timeSpan);
    }
}

