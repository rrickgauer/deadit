﻿namespace Deadit.Lib.Auth;

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
}
