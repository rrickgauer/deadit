using Deadit.Lib.Domain.Enum;

namespace Deadit.Lib.Domain.Model;

public class PostLink : Post
{
    public string? Url { get; set; }

    public override PostType PostType => PostType.Link;
}

