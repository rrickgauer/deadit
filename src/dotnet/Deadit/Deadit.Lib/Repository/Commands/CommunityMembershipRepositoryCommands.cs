namespace Deadit.Lib.Repository.Commands;

public sealed class CommunityMembershipRepositoryCommands
{
    /// <summary>
    /// @user_id
    /// </summary>
    public const string SelectAllByUserId = @"
        SELECT
            *
        FROM
            View_Community_Membership m
        WHERE
            m.user_id = @user_id
        ORDER BY
            m.user_joined_community_on DESC;";


    public const string SelectByUserIdCommunityId = @"
        SELECT
            *
        FROM
            View_Community_Membership vcb
        WHERE
            vcb.community_id = @community_id
            AND vcb.user_id = @user_id
        LIMIT
            1;";

    public const string SelectByUserIdCommunityName = @"
        SELECT
            *
        FROM
            View_Community_Membership vcb
        WHERE
            vcb.community_name = @community_name
            AND vcb.user_id = @user_id
        LIMIT
            1;";

    /// <summary>
    /// @user_id
    /// @community_name
    /// </summary>
    public const string DeleteByUserIdAndCommunityName = @"
    DELETE FROM
        Community_Membership
    WHERE
        user_id = @user_id
        AND community_id = (
            SELECT
                c.id
            FROM
                Community c
            WHERE
                c.name = @community_name
            LIMIT
                1
        );";


    /// <summary>
    /// @user_id
    /// @community_id
    /// </summary>
    public const string InsertByUserIdAndCommunityName = @"
        REPLACE INTO Community_Membership (user_id, community_id) (
            SELECT
                @user_id,
                c.id
            FROM
                Community c
            WHERE
                c.name = @community_name
            LIMIT
                1
        );";

}
