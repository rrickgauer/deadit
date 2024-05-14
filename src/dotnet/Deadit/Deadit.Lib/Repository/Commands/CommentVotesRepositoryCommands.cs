namespace Deadit.Lib.Repository.Commands;

public sealed class CommentVotesRepositoryCommands
{
    public const string SaveVote = @"
        INSERT INTO
            Vote_Comment (comment_id, user_id, vote_type, created_on)
        VALUES
            (@comment_id, @user_id, @vote_type, @created_on) AS new_values 
        ON DUPLICATE KEY UPDATE
            vote_type = new_values.vote_type;";

    public const string SelectCommentVotes = @"
        SELECT
            v.*
        FROM
            View_Vote_Comment v
        WHERE
            v.vote_comment_comment_id = @comment_id;";


    public const string SelectUserCommentVotesInPost = @"
        SELECT
            v.*
        FROM
            View_Vote_Comment v
        WHERE
            v.vote_comment_user_id = @user_id
            AND v.post_id = @post_id;";



    public const string SelectVote = @"
        SELECT
            v.*
        FROM
            View_Vote_Comment v
        WHERE
            v.vote_comment_comment_id = @comment_id
            AND v.vote_comment_user_id = @user_id
        LIMIT
            1;";


}
