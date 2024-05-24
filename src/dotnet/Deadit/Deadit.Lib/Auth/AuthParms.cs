using Deadit.Lib.Domain.Enum;

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
    }

    public class CommentAuthData
    {
        public required AuthPermissionType AuthPermissionType { get; set; }
        public required uint? UserId { get; set; }
        public required Guid CommentId { get; set; }
        public required Guid PostId { get; set; }
        public required string CommunityName { get; set; }
    }

    public class ModifyPostAuthData
    {
        public required Guid PostId { get; set; }
        public required uint ClientId { get; set; }
        public required string CommunityName { get; set; }
    }

    public class PostVoteAuthData
    {
        public required Guid PostId { get; set; }
    }

    public class CommentVoteAuthData
    {
        public required Guid CommentId { get; set; }
    }

}
