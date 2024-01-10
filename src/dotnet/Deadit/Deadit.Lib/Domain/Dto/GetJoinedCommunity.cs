namespace Deadit.Lib.Domain.Dto;

public class GetJoinedCommunity
{
    public uint? CommunityId { get; set; }
    public string? CommunityName { get; set; }
    public string? CommunityTitle { get; set; }
    public string? CommunityDescription { get; set; }
    public DateTime CommunityCreatedOn { get; set; } = DateTime.Now;
    public long CommunityCountMembers { get; set; } = 0;
    public DateTime UserJoinedOn { get; set; } = DateTime.Now;
}
