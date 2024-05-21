namespace Deadit.Lib.Repository.Commands;

public sealed class PostVotesRepositoryCommands
{
    public const string Save = @"
        INSERT INTO
            Vote_Post (post_id, user_id, vote_type, created_on)
        VALUES
            (@post_id, @user_id, @vote_type, @created_on) AS new_values 
        ON DUPLICATE KEY UPDATE
            vote_type = new_values.vote_type;";



    public const string SelectAllPostVotes = @"
        SELECT
            v.*
        FROM
            View_Vote_Post v
        WHERE
            v.vote_post_post_id = @post_id;";



    public const string SelectVote = @"
        SELECT
            v.*
        FROM
            View_Vote_Post v
        WHERE
            v.vote_post_post_id = @post_id
            AND v.vote_post_user_id = @user_id
        LIMIT
            1;";


    public const string SelectAllUserVotesInCommunity = @"
        SELECT
            v.*
        FROM
            View_Vote_Post v
        WHERE
            v.vote_post_user_id = @user_id
            AND v.post_community_name = @community_name;";



    public const string SelectUserPostVotesLimit = @"
        SELECT
            v.*
        FROM
            View_Vote_Post v
        WHERE
            v.vote_post_user_id = @user_id
            AND v.vote_post_post_id in {0};";

}
