﻿using Deadit.Lib.Domain.Attributes;

namespace Deadit.Lib.Domain.Enum;

public enum ErrorCode : uint
{
    #region - Signup -

    [ErrorCodeGroup(ErrorCodeGroup.AccountSignup)]
    SignUpEmailTaken = 200,
    
    [ErrorCodeGroup(ErrorCodeGroup.AccountSignup)]
    SignupUsernameTaken = 201,

    [ErrorCodeGroup(ErrorCodeGroup.AccountSignup)]
    SignupInvalidPassword = 202,

    #endregion

    #region - CommunitySettings -

    [ErrorCodeGroup(ErrorCodeGroup.CommunitySettings)]
    CommunitySettingsInvalidNameCharacter = 300,

    [ErrorCodeGroup(ErrorCodeGroup.CommunitySettings)]
    CommunitySettingsNameTaken = 301,

    [ErrorCodeGroup(ErrorCodeGroup.CommunitySettings)]
    CommunitySettingsNameBanned = 302,

    #endregion

    #region - Voting -

    [ErrorCodeGroup(ErrorCodeGroup.Voting)]
    VotingMissingRequiredParm = 400,

    [ErrorCodeGroup(ErrorCodeGroup.Voting)]
    VotingBothParmsGiven = 401,

    #endregion

    #region - Comment -
    [ErrorCodeGroup(ErrorCodeGroup.Comment)]
    CommentInvalidParentId = 500,

    [ErrorCodeGroup(ErrorCodeGroup.Comment)]
    CommentParentCommentIsLocked = 501,

    [ErrorCodeGroup(ErrorCodeGroup.Comment)]
    CommentPostLocked = 502,

    [ErrorCodeGroup(ErrorCodeGroup.Comment)]
    CommentPostRemoved = 503,

    [ErrorCodeGroup(ErrorCodeGroup.Comment)]
    CommentPostDeleted = 504,

    #endregion


    #region - Post -
    [ErrorCodeGroup(ErrorCodeGroup.Post)]
    PostTextPostContentNotAllowed = 600,

    [ErrorCodeGroup(ErrorCodeGroup.Post)]
    PostTextPostContentRequired = 601,

    #endregion
}
