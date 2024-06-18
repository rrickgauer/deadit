using Deadit.Lib.Domain.Enum;

namespace Deadit.Lib.Auth.AuthParms;

public class CommentAuthData
{
    public required AuthPermissionType AuthPermissionType { get; set; }
    public required uint? UserId { get; set; }
    public required Guid CommentId { get; set; }
    public required Guid? PostId { get; set; }
    public required Guid? ParentCommentId { get; set; }
}
