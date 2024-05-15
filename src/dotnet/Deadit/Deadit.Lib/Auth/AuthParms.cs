namespace Deadit.Lib.Auth;

public class AuthParms
{
    public class CreatePostAuthData
    {
        public required uint UserId { get; set; }
        public required string CommunityName { get; set; }
    }

    public class GetPostAuthData
    {
        public required Guid PostId { get; set; }
        public required string CommunityName { get; set; }
    }

    public class CommentAuthData
    {
        public required bool IsDelete { get; set; }
        public required uint UserId { get; set; }
        public required Guid CommentId { get; set; }
    }
}
