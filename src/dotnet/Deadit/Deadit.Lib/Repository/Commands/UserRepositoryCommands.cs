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
            View_User u
        WHERE
            u.user_username = @username
            AND u.user_password = @password
        LIMIT
            1;";


    public const string SelectUserById = @"
        SELECT
            u.*
        FROM
            View_User u
        WHERE
            u.user_id = @user_id
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
            u.user_email = @email
            OR u.user_username = @username
        LIMIT
            2;";
}

