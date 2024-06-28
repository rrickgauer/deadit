namespace Deadit.Lib.Repository.Commands;

public sealed class CommunityRepositoryCommands
{
    public const string SelectCommunityByName = @"
        SELECT
            *
        FROM
            View_Community c
        WHERE
            c.community_name = @community_name
        LIMIT
            1;";

    public const string SelectCommunityById = @"
        SELECT
            *
        FROM
            View_Community c
        WHERE
            c.community_id = @id
        LIMIT
            1;";




    /// <summary>
    /// @name,
    /// @title,
    /// @owner_id,
    /// @description
    /// </summary>
    public const string InsertCommunity = @"
        INSERT INTO
            Community (name, title, owner_id, description)
        VALUES
            (@name, @title, @owner_id, @description);";



    public const string SelectUserCreatedCommunities = @"
        SELECT
            v.*
        FROM
            View_Community v
        WHERE
            v.community_owner_id = @user_id
        ORDER BY
            v.community_name ASC;";



    public const string UpdateCommunity = @"
        UPDATE
            Community
        SET
            title = @title,
            description = @description,
            community_type = @community_type,
            text_post_body_rule = @text_post_body_rule,
            membership_closed_on = @membership_closed_on,
            flair_post_rule = @flair_post_rule
        WHERE
            id = @id;";


}
