

export enum ApiErrorCode
{

    //#region - Signup -

    SignUpEmailTaken = 200,
    SignupUsernameTaken = 201,
    SignupInvalidPassword = 202,

    //#endregion

    
    //#region - Create Community -

    CommunitySettingsInvalidNameCharacter = 300,
    CommunitySettingsNameTaken = 301,
    CommunitySettingsNameBanned = 302,
    CommunitySettingsPrivateCommunityAccessAttempt = 303,

    //#endregion


    //#region - Voting -

    VotingMissingRequiredParm = 400,
    VotingBothParmsGiven = 401,

    //#endregion


    //#region - Comment -

    CommentInvalidParentId = 500,
    CommentParentCommentIsLocked = 501,
    CommentPostLocked = 502,
    CommentPostRemoved = 503,
    CommentPostDeleted = 504,

    //#endregion

    //#region - Post -

    PostTextPostContentNotAllowed = 600,
    PostTextPostContentRequired = 601,
    PostInvalidFlairPostId = 602,
    PostFlairRequired = 603,
    PostFlairNotAllowed = 604,

    //#endregion


    //#region - Flair Post -

    FlairPostNameContainsInvalidCharacter = 700,
    FlairPostNameIsBanned = 701,
    FlairPostDuplicateName = 702,
    FlairPostInvalidColor = 703,

    //#endregion

}

