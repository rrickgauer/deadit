namespace Deadit.Lib.Domain.Model;

public class CommunityMembership
{
    public uint? CommunityId { get; set; }
    public uint? UserId { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.Now;
}
