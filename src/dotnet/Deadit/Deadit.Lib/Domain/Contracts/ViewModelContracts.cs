using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Paging;

namespace Deadit.Lib.Domain.Contracts;



public interface IClientId
{
    public uint? ClientId { get; set; }
}

public interface ICommunityName
{
    public string CommunityName { get; set; }
}

public interface IPostId
{
    public Guid PostId { get; set; }
}

public interface ISortOption
{
    public SortOption SortOption { get; set; }
}

public interface IPostPagination
{
    public PaginationPosts Pagination { get; set; }
}


public interface IClientLoggedIn
{
    public bool IsLoggedIn { get; set; }
}

public interface IPostAuthor
{
    public bool IsPostAuthor { get; set; }
}


public interface IPostDeleted
{
    public bool PostIsDeleted { get; }
}

public interface IClientPostVote
{
    public VoteType ClientPostVote { get; }
}

