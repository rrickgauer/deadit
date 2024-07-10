using Deadit.Lib.Domain.TableView;

namespace Deadit.Lib.Domain.ViewModel;

public class NewPostPageViewModel
{
    public required ViewCommunity Community { get; set; }
    public required List<ViewFlairPost> FlairPosts { get; set; }
}
