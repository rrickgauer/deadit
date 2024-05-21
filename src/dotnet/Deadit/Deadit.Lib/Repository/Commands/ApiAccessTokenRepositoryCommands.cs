namespace Deadit.Lib.Repository.Commands;

public sealed class ApiAccessTokenRepositoryCommands
{
    public const string SelectToken = @"
        SELECT
            v.*
        FROM
            View_Api_Access_Token v
        WHERE
            v.token_token = @token
        LIMIT
            1;";


}
