using Deadit.Lib.Domain.Enum;

namespace Deadit.Lib.Domain.Forms;

public class GetMembersSettingsPageQueryParms
{
    public CommunityMembersSortField? Sort { get; set; }
    public SortDirection? SortDirection { get; set; }
    public uint? Page { get; set; }
}
