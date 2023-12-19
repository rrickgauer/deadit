namespace Deadit.Lib.Repository.Commands;

public sealed class UserRepositoryCommands
{
    /// <summary>
    /// @username
    /// @password
    /// </summary>
    public const string SelectUserByLoginInfo = @"
        SELECT
            u.*
        FROM
            User u
        WHERE
            u.username = @username
            AND u.password = @password
        LIMIT
            1;";
}
