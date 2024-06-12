using Deadit.Lib.Domain.Other;
using Microsoft.AspNetCore.Html;
using System.ComponentModel;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Web;

namespace Deadit.Lib.Domain.Enum;


[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CommunityMembersSortOptions
{

    [Description("Username (Asc)")]
    UsernameAsc,

    [Description("Username (Desc)")]
    UsernameDesc,

    [Description("Joined On (Asc)")]
    JoinedOnAsc,

    [Description("Joined On (Desc)")]
    JoinedOnDesc,
}


public static class CommunityMembersSortOptionsExtensions
{

    public static HtmlString GetOptionHtml(this CommunityMembersSortOptions option, CommunityMembersSorting sorting)
    {

        string selected = string.Empty;

        if (option == sorting.GetOption())
        {
            selected = "selected";
        }

        string result = $"<option value={option.GetName()} {selected}>{option.GetDisplay()}</option>";

        return new(result);
    }

    public static string GetDisplay(this CommunityMembersSortOptions option)
    {
        var attr = typeof(CommunityMembersSortOptions).GetField(option.GetName())?.GetCustomAttribute<DescriptionAttribute>();

        var display = attr?.Description ?? option.GetName();

        return display;
    }

    public static string GetName(this CommunityMembersSortOptions option)
    {
        return System.Enum.GetName(option)!;
    }



    
}
