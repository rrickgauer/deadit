using Deadit.Lib.Domain.Contracts;
using Deadit.Lib.Domain.Dto;
using Deadit.Lib.Domain.Other;
using Deadit.Lib.Domain.Paging;

namespace Deadit.Lib.Domain.ViewModel;

public class HomePageViewModel : IClientId
{
    public required uint? ClientId { get; set; }
    public bool IsLoggedIn => ClientId.HasValue;
    public required List<GetPostUserVoteDto> Posts { get; set; } = new();
    public required PostSorting PostSort { get; set; }
    public required PaginationPosts Pagination { get; set; }
}
