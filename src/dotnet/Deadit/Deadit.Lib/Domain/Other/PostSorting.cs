using Deadit.Lib.Domain.Enum;

namespace Deadit.Lib.Domain.Other;

public class PostSorting(PostSortType postSort)
{
    public PostSortType PostSortType { get; set; } = postSort;
    public TopPostSort TopSort { get; set; } = TopPostSort.Day;

    public bool SortByNew => PostSortType == PostSortType.New;

    public PostSorting(PostSortType postSort, TopPostSort topSort) : this(postSort)
    {
        TopSort = topSort; 
    }
}
