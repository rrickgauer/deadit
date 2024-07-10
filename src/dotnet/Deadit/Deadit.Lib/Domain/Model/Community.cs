using Deadit.Lib.Domain.Enum;

namespace Deadit.Lib.Domain.Model;

public class Community
{
    public uint? Id { get; set; }
    public string? Name { get; set; }
    public string? Title { get; set; }
    public uint? OwnerId { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.Now;
    public CommunityType CommunityType { get; set; } = CommunityType.Private;
    public TextPostBodyRule TextPostBodyRule { get; set; } = TextPostBodyRule.Optional;
    public DateTime? MembershipClosedOn { get; set; }
    public FlairPostRule FlairPostRule { get; set; } = FlairPostRule.Optional;
}

