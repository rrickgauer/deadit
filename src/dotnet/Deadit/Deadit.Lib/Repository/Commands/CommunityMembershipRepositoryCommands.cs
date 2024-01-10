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


}
