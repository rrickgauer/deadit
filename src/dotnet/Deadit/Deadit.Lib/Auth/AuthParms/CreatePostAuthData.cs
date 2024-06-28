using Deadit.Lib.Domain.Enum;

namespace Deadit.Lib.Auth.AuthParms;

public class CreatePostAuthData
{
    public required uint UserId { get; set; }
    public required string CommunityName { get; set; }
    public PostType? PostType { get; set; }
    public string? TextPostContent { get; set; }
    public required uint? FlairPostId { get; set; }
}
