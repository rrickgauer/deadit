namespace Deadit.Lib.Domain.Paging;

public class PaginationPosts(uint? page) : Pagination(page)
{
    public override uint Limit => 10;
}
