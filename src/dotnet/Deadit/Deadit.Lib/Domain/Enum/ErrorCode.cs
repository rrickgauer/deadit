namespace Deadit.Lib.Domain.Enum;

public enum ErrorCode : int
{
    SignUpEmailTaken = 1,
    SignupUsernameTaken = 2,
    SignupInvalidPassword = 3,

    CreateCommunityInvalidNameCharacter = 4,
    CreateCommunityNameTaken = 5,

    ValidationError = 6,

    CreateCommunityNameBanned = 7,
}
