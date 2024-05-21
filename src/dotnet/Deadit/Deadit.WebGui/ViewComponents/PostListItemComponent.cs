using Deadit.Lib.Domain.Constants;
using Deadit.Lib.Domain.ViewModel;

namespace Deadit.WebGui.ViewComponents;

public class PostListItemComponent : ViewComp<PostListItemViewModel>
{
    public override string RazorFileName => ViewComponentFiles.PostListItemView;
}
