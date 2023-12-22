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


    public const string SelectUserById = @"
        SELECT
            u.*
        FROM
            View_User u
        WHERE
            u.id = @user_id
        LIMIT
            1;";


    /// <summary>
    /// @email
    /// @username
    /// @password
    /// </summary>
    public const string InsertUser = @"
        INSERT INTO
            User (email, username, password)
        VALUES
            (@email, @username, @password);";

    /// <summary>
    /// @email
    /// @username
    /// </summary>
    public const string SelectByEmailOrUsername = @"
        SELECT
            *
        FROM
            View_User u
        WHERE
            u.email = @email
            OR u.username = @username
        LIMIT
            2;";
}

