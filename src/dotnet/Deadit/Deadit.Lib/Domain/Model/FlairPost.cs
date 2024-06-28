namespace Deadit.Lib.Domain.Model;


public class FlairPost
{
    public uint? Id { get; set; }
    public uint? CommunityId { get; set; }
    public string? Name { get; set; }
    public string? Color { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
}

