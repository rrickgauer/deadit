using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.TableView;
using static Deadit.Lib.Domain.ViewModel.ViewModelContracts;

namespace Deadit.Lib.Domain.ViewModel;

public class PostPageViewModel : IAuthor, IClientId, ILoggedIn, ISortOption
{
    public required ViewPost Post { get; set; }
    public required bool IsAuthor { get; set; }
    public required uint? ClientId { get; set; }
    public required bool IsLoggedIn { get; set; } 
    public required List<ViewComment> Comments { get; set; }
    public required SortOption SortOption { get; set; }
}


