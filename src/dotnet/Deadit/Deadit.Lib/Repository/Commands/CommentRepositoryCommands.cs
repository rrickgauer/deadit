namespace Deadit.Lib.Repository.Commands;

public sealed class CommentRepositoryCommands
{
    public const string SelectAllPostCommentsSortedTemplate = @"
        SELECT
            c.*
        FROM
            View_Comment c
        WHERE
            c.comment_post_id = @post_id
        ORDER BY
            {0};";


    public const string SelectComment = @"
        SELECT
            c.*
        FROM
            View_Comment c
        WHERE
            c.comment_id = @comment_id
        LIMIT
            1;";


    public const string Save = @"
        INSERT INTO Comment 
        (
            id,
            author_id,
            post_id,
            content,
            parent_id,
            created_on
        )
        VALUES
        (
            @id,
            @author_id,
            @post_id,
            @content,
            @parent_id,
            @created_on
        ) AS new_values 
        ON DUPLICATE KEY UPDATE
            content = new_values.content;";


    public const string SetDeleted = @"
        UPDATE
            Comment
        SET
            deleted_on = UTC_TIMESTAMP()
        WHERE
            id = @comment_id;";


    public const string SaveModerate = @"
        UPDATE
            Comment
        SET
            locked_on = @locked_on,
            removed_on = @removed_on
        WHERE
            id = @comment_id;";


}
