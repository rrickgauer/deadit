
using Deadit.Lib.Domain.TableView;
using static Deadit.Lib.Domain.ViewModel.ViewModelContracts;

namespace Deadit.Lib.Domain.ViewModel;

public class CommunityPageViewModel : ILoggedIn
{
    public required ViewCommunity Community { get; set; }
    public bool IsMember { get; set; } = false;
    public bool IsLoggedIn { get; set; } = false;

    public required List<ViewPost> Posts { get; set; }
}
