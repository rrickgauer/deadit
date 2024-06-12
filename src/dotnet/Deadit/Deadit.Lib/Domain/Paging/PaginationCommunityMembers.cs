namespace Deadit.Lib.Domain.Paging;

public class PaginationCommunityMembers(uint? page) : Pagination(page)
{
    public override uint Limit => 100;
}


