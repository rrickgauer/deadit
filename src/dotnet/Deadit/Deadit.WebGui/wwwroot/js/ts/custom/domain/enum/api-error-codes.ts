

export enum ApiErrorCode
{

    //#region - Signup -

    SignUpEmailTaken = 200,
    SignupUsernameTaken = 201,
    SignupInvalidPassword = 202,

    //#endregion

    
    //#region - Create Community -

    CreateCommunityInvalidNameCharacter = 300,
    CreateCommunityNameTaken = 301,
    CreateCommunityNameBanned = 302,

    //#endregion
}

