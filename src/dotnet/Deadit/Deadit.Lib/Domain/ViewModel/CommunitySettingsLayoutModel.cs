using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Other;
using Deadit.Lib.Domain.Paging;
using Deadit.Lib.Domain.TableView;

namespace Deadit.Lib.Domain.ViewModel;

public class CommunitySettingsLayoutModel
{
    public required string PageTitle { get; set; }
    public required ViewCommunity Community { get; set; }
    public required ActiveCommunitySettingsPage ActivePage { get; set; }
}

public class CommunitySettingsLayoutModel<T> : CommunitySettingsLayoutModel
{
    public required T PageModel { get; set; }
}

public class GeneralCommunitySettingsPageModel
{
    
}


public class ContentCommunitySettingsPageModel
{

}

public class MembersCommunitySettingsPageModel
{
    public required List<ViewCommunityMembership> Memberships { get; set; }
    public required CommunityMembersSorting Sorting { get; set; }
    public required PaginationCommunityMembers Pagination { get; set; }
}
