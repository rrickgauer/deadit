using Deadit.Lib.Domain.Enum;

namespace Deadit.Lib.Domain.Model;

public abstract class Post
{
    public abstract PostType PostType { get; }

    public Guid? Id { get; set; }
    public uint? CommunityId { get; set; }
    public string? Title { get; set; }
    public uint? AuthorId { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.Now;

    public ushort PostTypeValue => (ushort)PostType;
}

