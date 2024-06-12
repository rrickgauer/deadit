

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

}

