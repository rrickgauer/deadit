using Deadit.Lib.Domain.Contracts;
using Deadit.Lib.Domain.Dto;
using Deadit.Lib.Domain.Other;
using Deadit.Lib.Domain.Paging;
using Deadit.Lib.Domain.TableView;

namespace Deadit.Lib.Domain.ViewModel;

public class CommunityPageViewModel : IClientLoggedIn
{
    public required ViewCommunity Community { get; set; }
    public bool IsMember { get; set; } = false;
    public bool IsLoggedIn { get; set; } = false;
    public required List<GetPostUserVoteDto> PostDtos { get; set; } = new();
    public required PostSorting PostSort { get; set; }
    public required PaginationPosts Pagination { get; set; }
}

