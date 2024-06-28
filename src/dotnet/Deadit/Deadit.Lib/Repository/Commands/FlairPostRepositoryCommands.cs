namespace Deadit.Lib.Repository.Commands;

public sealed class FlairPostRepositoryCommands
{
    public const string SelectAllByCommunityId = @"
        SELECT
            v.*
        FROM
            View_Flair_Post v
        WHERE
            v.flair_post_community_id = @community_id
        ORDER BY
            v.flair_post_name ASC;";


    public const string SelectAllByCommunityName = @"
        SELECT
            v.*
        FROM
            View_Flair_Post v
        WHERE
            v.community_name = @community_name
        ORDER BY
            v.flair_post_name ASC;";

    public const string SelectById = @"
        SELECT
            v.*
        FROM
            View_Flair_Post v
        WHERE
            v.flair_post_id = @flair_id
        LIMIT
            1;";


    public const string Insert = @"
        INSERT INTO
            Flair_Post (id, community_id, name, color, created_on)
        VALUES
            (@id, @community_id, @name, @color, @created_on);";

    public const string Update = @"
        UPDATE
            Flair_Post
        SET
            name = @name,
            color = @color
        WHERE
            id = @id;";


    public const string Delete = @"
        DELETE FROM
            Flair_Post
        WHERE
            id = @id;";

}
