using Deadit.Lib.Domain.Enum;

namespace Deadit.Lib.Domain.Model;

public class PostText : Post
{
    public override PostType PostType => PostType.Text;

    public string? Content { get; set; }
}

