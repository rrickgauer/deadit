using Deadit.Lib.Domain.Enum;

namespace Deadit.Lib.Domain.Other;

public class CommunityMembersSorting(CommunityMembersSortField? sortType, SortDirection? direction)
{
    public CommunityMembersSortField SortType { get; } = sortType ?? CommunityMembersSortField.Username;
    public SortDirection SortDirection { get; } = direction ?? SortDirection.Ascending;

    public string GetSqlOrderClause()
    {
        return $"{SortType.GetSqlColumn()} {SortDirection.GetSqlValue()}";
    }

    public CommunityMembersSortOptions GetOption()
    {

        if (SortType == CommunityMembersSortField.Username)
        {
            if (SortDirection == SortDirection.Ascending)
            {
                return CommunityMembersSortOptions.UsernameAsc;
            }
            else
            {
                return CommunityMembersSortOptions.UsernameDesc;
            }
        }
        else
        {
            if (SortDirection == SortDirection.Ascending)
            {
                return CommunityMembersSortOptions.JoinedOnAsc;
            }
            else
            {
                return CommunityMembersSortOptions.JoinedOnDesc;
            }
        }
    }


}
