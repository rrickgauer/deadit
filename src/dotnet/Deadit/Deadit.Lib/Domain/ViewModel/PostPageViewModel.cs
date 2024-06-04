using Deadit.Lib.Domain.Contracts;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.TableView;

namespace Deadit.Lib.Domain.ViewModel;

public class PostPageViewModel : IPostAuthor, IClientId, IClientLoggedIn, ISortOption
{
    public required ViewPost Post { get; set; }
    public required bool IsPostAuthor { get; set; }
    public required uint? ClientId { get; set; } 
    public required bool IsLoggedIn { get; set; } 
    public required List<ViewComment> Comments { get; set; }
    public required SortOption SortOption { get; set; }
    public required VoteType UserPostVote { get; set; }
    public required bool IsCommunityModerator { get; set; }
}


