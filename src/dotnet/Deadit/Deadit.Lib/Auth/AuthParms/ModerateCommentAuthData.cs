namespace Deadit.Lib.Auth.AuthParms;

public class ModerateCommentAuthData
{
    public required Guid CommentId { get; set; }
    public required uint ClientId { get; set; }
}
