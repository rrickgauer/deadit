namespace Deadit.Lib.Repository.Commands;

public sealed class BannedCommunityNameRepositoryCommands
{
    public const string SelectAll = @"
        SELECT
            bn.id AS id,
            bn.name AS name
        FROM
            Banned_Community_Name bn
        ORDER BY
            bn.id ASC;";
}
