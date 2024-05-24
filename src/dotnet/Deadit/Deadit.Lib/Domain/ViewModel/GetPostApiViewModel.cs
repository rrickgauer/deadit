using Deadit.Lib.Domain.Contracts;
using Deadit.Lib.Domain.Dto;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.TableView;

namespace Deadit.Lib.Domain.ViewModel;

public class GetPostApiViewModel : IClientLoggedIn, IPostDeleted, IClientPostVote
{
    public required bool IsLoggedIn { get; set; }
    public required bool IsCommunityModerator { get; set; }
    public required bool IsPostAuthor { get; set; }
    public required bool PostIsDeleted { get; set; }
    public required bool PostIsLocked { get; set; }
    public required bool IsPostModRemoved { get; set; }
    public required ViewPost Post { get; set; }
    public required VoteType ClientPostVote { get; set; }
    public required List<GetCommentDto> Comments { get; set; }

}


