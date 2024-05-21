using Deadit.Lib.Domain.Dto;

namespace Deadit.Lib.Domain.ViewModel;

public class PostListItemViewModel
{
    public required GetPostUserVoteDto Post { get; set; }
    public required bool IsLoggedIn { get; set; }
}

