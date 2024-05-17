using Deadit.Lib.Domain.Attributes;

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

    #region - Create Community -

    [ErrorCodeGroup(ErrorCodeGroup.CreateCommunity)]
    CreateCommunityInvalidNameCharacter = 300,

    [ErrorCodeGroup(ErrorCodeGroup.CreateCommunity)]
    CreateCommunityNameTaken = 301,

    [ErrorCodeGroup(ErrorCodeGroup.CreateCommunity)]
    CreateCommunityNameBanned = 302,

    #endregion

    #region - Voting -

    [ErrorCodeGroup(ErrorCodeGroup.Voting)]
    VotingMissingRequiredParm = 400,

    [ErrorCodeGroup(ErrorCodeGroup.Voting)]
    VotingBothParmsGiven = 401,

    #endregion
}
