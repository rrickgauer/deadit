using Deadit.Lib.Domain.Enum;

namespace Deadit.Lib.Domain.Other;

public class PostSorting(CommunityPagePostSort postSort)
{
    public CommunityPagePostSort PostSort { get; set; } = postSort;
    public TopPostSort TopSort { get; set; } = TopPostSort.Day;

    public bool SortByNew => PostSort == CommunityPagePostSort.New;

    public PostSorting(CommunityPagePostSort postSort, TopPostSort topSort) : this(postSort)
    {
        TopSort = topSort; 
    }
}
