namespace Deadit.Lib.Domain.Model;

public class Community
{
    public uint? Id { get; set; }
    public string? Name { get; set; }
    public string? Title { get; set; }
    public int? OwnerId { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.Now;
}

