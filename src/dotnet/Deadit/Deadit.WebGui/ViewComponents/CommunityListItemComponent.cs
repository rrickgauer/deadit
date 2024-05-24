using Deadit.Lib.Domain.Constants;
using Deadit.Lib.Domain.ViewModel;

namespace Deadit.WebGui.ViewComponents;

public class CommunityListItemComponent : ViewComp<CommunityListItemViewModel>
{
    public override string RazorFileName => ViewComponentFiles.CommunityListItemView;
}