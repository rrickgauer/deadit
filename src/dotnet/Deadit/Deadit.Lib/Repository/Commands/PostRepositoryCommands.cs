﻿using Deadit.Lib.Domain.Constants;

namespace Deadit.Lib.Repository.Commands;

public sealed class PostRepositoryCommands
{

    private const string _selectNewestCommunityPostsTemplate = @"
        SELECT
            p.*
        FROM
            View_Post p
        WHERE
            p.community_name = @community_name
        ORDER BY 
            p.post_created_on DESC
        {0};";

    public static readonly string SelectNewestCommunityPosts = string.Format(_selectNewestCommunityPostsTemplate, string.Empty);
    public static readonly string SelectNewestCommunityPostsLimit = string.Format(_selectNewestCommunityPostsTemplate, RepositoryConstants.PAGINATION_CLAUSE);


    private const string _selectTopCommunityPostsTemplate = @"
        SELECT
            p.*
        FROM
            View_Post p
        WHERE
            p.community_name = @community_name
            AND p.post_created_on > @created_on
        ORDER BY
            p.post_count_votes_score DESC
        {0};";

    public static readonly string SelectTopCommunityPosts = string.Format(_selectTopCommunityPostsTemplate, string.Empty);
    public static readonly string SelectTopCommunityPostsLimit = string.Format(_selectTopCommunityPostsTemplate, RepositoryConstants.PAGINATION_CLAUSE);




    public const string SelectAllCommunityLink = @"
        SELECT
            *
        FROM
            View_Post_Link p
        WHERE
            p.community_name = @community_name;";


    public const string SelectAllCommunityText = @"
        SELECT
            *
        FROM
            View_Post_Text p
        WHERE
            p.community_name = @community_name;";



    public const string Select = @"
        SELECT
            p.*
        FROM
            View_Post p
        WHERE
            p.post_id = @post_id;";


    public const string SelectPostText = @"
        SELECT
            p.*
        FROM
            View_Post_Text p
        WHERE
            p.post_id = @post_id
        LIMIT
            1;";

    public const string SelectPostLink = @"
        SELECT
            p.*
        FROM
            View_Post_Link p
        WHERE
            p.post_id = @post_id
        LIMIT
            1;";



    public const string SavePost = @"
        INSERT INTO Post 
        (
            id,
            community_id,
            title,
            post_type,
            author_id,
            created_on
        )
        VALUES
        (
            @id,
            @community_id,
            @title,
            @post_type,
            @author_id,
            @created_on
        ) AS new_values 
        ON duplicate KEY UPDATE
            title = new_values.title;";


    public const string SavePostText = @"
        INSERT INTO
            Post_Text (id, content)
        VALUES
            (@id, @content) AS new_values 
        ON DUPLICATE KEY UPDATE
            content = new_values.content;";



    public const string SavePostLink = @"
        INSERT INTO
            Post_Link (id, url)
        VALUES
            (@id, @url) AS new_values 
        ON DUPLICATE KEY UPDATE
            url = new_values.url;";
}
