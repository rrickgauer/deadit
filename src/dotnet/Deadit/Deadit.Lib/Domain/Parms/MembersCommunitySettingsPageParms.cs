using Deadit.Lib.Domain.Other;
using Deadit.Lib.Domain.Paging;

namespace Deadit.Lib.Domain.Parms;

public class MembersCommunitySettingsPageParms : GeneralCommunitySettingsPageParms
{
    public required CommunityMembersSorting CommunityMembersSorting { get; set; }
    public required PaginationCommunityMembers Pagination { get; set; }
}

