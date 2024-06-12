using Deadit.Lib.Domain.Enum;

namespace Deadit.Lib.Domain.Dto;

public class GetJoinedCommunity
{
    public uint? CommunityId { get; set; }
    public string? CommunityName { get; set; }
    public string? CommunityTitle { get; set; }
    public string? CommunityDescription { get; set; }
    public DateTime CommunityCreatedOn { get; set; } = DateTime.Now;
    public long CommunityCountMembers { get; set; } = 0;
    public CommunityType CommunityType { get; set; } = CommunityType.Private;
    public TextPostBodyRule CommunityTextPostBodyRule { get; set; } = TextPostBodyRule.Optional;
    public DateTime? CommunityMembershipClosedOn { get; set; }

    public DateTime UserJoinedOn { get; set; } = DateTime.Now;
}
