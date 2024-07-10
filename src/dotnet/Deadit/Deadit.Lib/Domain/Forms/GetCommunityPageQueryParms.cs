using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Other;

namespace Deadit.Lib.Domain.Forms;

public abstract class BaseGetCommunityPageQueryParms
{
    public uint? Page { get; set; }
    public uint? Flair { get; set; }

    public abstract PostSorting GetPostSorting();
}


public class GetCommunityPageQueryParms : BaseGetCommunityPageQueryParms
{
    public override PostSorting GetPostSorting()
    {
        return new(PostSortType.New);
    }
}

public class GetCommunityPageQueryParmsTop : BaseGetCommunityPageQueryParms
{
    public TopPostSort? Sort { get; set; }

    public override PostSorting GetPostSorting()
    {
        // defaults to Month if no query parm was given
        TopPostSort topSort = Sort ?? TopPostSort.Month;

        PostSorting postSorting = new(PostSortType.Top, topSort);

        return postSorting;
    }
}

