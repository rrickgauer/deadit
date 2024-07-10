using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Forms;
using Deadit.Lib.Domain.Model;
using Deadit.Lib.Domain.Paging;
using Deadit.Lib.Repository.Commands;
using Deadit.Lib.Repository.Contracts;
using Deadit.Lib.Repository.Other;
using Microsoft.Extensions.Hosting;
using MySql.Data.MySqlClient;
using System.Data;

namespace Deadit.Lib.Repository.Implementations;


[AutoInject<IPostRepository>(AutoInjectionType.Scoped, InjectionProject.Always)]
public class PostRepository(DatabaseConnection connection, TransactionConnection transactionConnection) : IPostRepository
{
    #region - Private Members -
    private readonly DatabaseConnection _connection = connection;
    private readonly TransactionConnection _transactionConnection = transactionConnection;
    #endregion

    #region - User Home Feed -

    public async Task<DataTable> SelectUserNewHomePostsAsnc(uint clientId, PaginationPosts pagination)
    {
        MySqlCommand command = new(PostRepositoryCommands.SelectNewUserHomePostsLimit);

        command.Parameters.AddWithValue("@user_id", clientId);

        command.AddPaginationParamters(pagination);

        return await _connection.FetchAllAsync(command);
    }

    public async Task<DataTable> SelectUserTopHomePostsAsnc(uint clientId, PaginationPosts pagination, DateTime createdAfter)
    {
        MySqlCommand command = new(PostRepositoryCommands.SelectTopHomePagePostsLimit);

        command.Parameters.AddWithValue("@user_id", clientId);
        command.Parameters.AddWithValue("@created_on", createdAfter);

        command.AddPaginationParamters(pagination);

        return await _connection.FetchAllAsync(command);
    }

    #endregion

    #region - Select Single Post -

    public async Task<DataRow?> SelectPostAsync(Guid postId)
    {
        MySqlCommand command = new(PostRepositoryCommands.Select);

        command.Parameters.AddWithValue("@post_id", postId);

        return await _connection.FetchAsync(command);
    }


    public async Task<DataRow?> SelectPostTextAsync(Guid postId)
    {
        MySqlCommand command = new(PostRepositoryCommands.SelectPostText);

        command.Parameters.AddWithValue("@post_id", postId);

        return await _connection.FetchAsync(command);
    }


    public async Task<DataRow?> SelectPostLinkAsync(Guid postId)
    {
        MySqlCommand command = new(PostRepositoryCommands.SelectPostLink);

        command.Parameters.AddWithValue("@post_id", postId);

        return await _connection.FetchAsync(command);
    }

    #endregion

    #region - Select Newest Community Posts -

    public async Task<DataTable> SelectNewestCommunityPostsAsync(string communityName, PaginationPosts pagination, uint flairId)
    {
        MySqlCommand command = new(PostRepositoryCommands.SelectNewestCommunityPostsLimitWithFlair);

        command.Parameters.AddWithValue("@community_name", communityName);
        command.Parameters.AddWithValue("@flair_id", flairId);

        command.AddPaginationParamters(pagination);

        return await _connection.FetchAllAsync(command);
    }

    public async Task<DataTable> SelectNewestCommunityPostsAsync(string communityName, PaginationPosts pagination)
    {
        MySqlCommand command = new(PostRepositoryCommands.SelectNewestCommunityPostsLimit);

        command.Parameters.AddWithValue("@community_name", communityName);

        command.AddPaginationParamters(pagination);

        return await _connection.FetchAllAsync(command);
    }

    public async Task<DataTable> SelectNewestCommunityPostsAsync(string communityName)
    {
        MySqlCommand command = new(PostRepositoryCommands.SelectNewestCommunityPosts);

        command.Parameters.AddWithValue("@community_name", communityName);

        return await _connection.FetchAllAsync(command);
    }

    #endregion

    #region - Select Top Community Posts -

    public async Task<DataTable> SelectTopCommunityPostsAsync(string communityName, DateTime createdAfter, PaginationPosts pagination, uint flairId)
    {
        MySqlCommand command = new(PostRepositoryCommands.SelectTopCommunityPostsLimitWithFlair);

        return await SelectTopCommunityPostsAsync(command, communityName, createdAfter, pagination, flairId);
    }

    public async Task<DataTable> SelectTopCommunityPostsAsync(string communityName, DateTime createdAfter, PaginationPosts pagination)
    {
        MySqlCommand command = new(PostRepositoryCommands.SelectTopCommunityPostsLimit);

        return await SelectTopCommunityPostsAsync(command, communityName, createdAfter, pagination);
    }

    public async Task<DataTable> SelectTopCommunityPostsAsync(string communityName, DateTime createdAfter)
    {
        MySqlCommand command = new(PostRepositoryCommands.SelectTopCommunityPosts);

        return await SelectTopCommunityPostsAsync(command, communityName, createdAfter);
    }

    private async Task<DataTable> SelectTopCommunityPostsAsync(MySqlCommand command, string communityName, DateTime createdAfter, PaginationPosts? pagination = null, uint? flairId = null)
    {
        command.Parameters.AddWithValue("@community_name", communityName);
        command.Parameters.AddWithValue("@created_on", createdAfter);

        if (pagination != null)
        {
            command.AddPaginationParamters(pagination);
        }

        if (flairId.HasValue)
        {
            command.Parameters.AddWithValue("@flair_id", flairId.Value);
        }

        return await _connection.FetchAllAsync(command);
    }


    #endregion

    #region - Insert -

    public async Task<List<int>> InsertPostAsync(PostText post)
    {
        // start the transaction
        await _transactionConnection.StartTransactionAsync();

        // insert the post base record
        int numRecords1 = await InsertPostBaseAsync(post, _transactionConnection);

        // insert the post text record
        int numRecords2 = await InsertPostTextAsync(post, _transactionConnection);

        // commit the commands
        await _transactionConnection.CommitAsync();

        // combine the results
        return new() { numRecords1, numRecords2 };
    }

    private static async Task<int> InsertPostTextAsync(PostText post, TransactionConnection transaction)
    {
        MySqlCommand command = new(PostRepositoryCommands.SavePostText);

        command.Parameters.AddWithValue("@id", post.Id);
        command.Parameters.AddWithValue("@content", post.Content);

        return await transaction.ExecuteInTransactionAsync(command);
    }

    public async Task<List<int>> InsertPostAsync(PostLink post)
    {
        // start the transaction
        await _transactionConnection.StartTransactionAsync();

        // insert the post base record
        int numRecords1 = await InsertPostBaseAsync(post, _transactionConnection);

        // insert the post text record
        int numRecords2 = await InsertPostLinkAsync(post, _transactionConnection);

        // commit the commands
        await _transactionConnection.CommitAsync();

        // combine the results
        return new() { numRecords1, numRecords2 };
    }

    private static async Task<int> InsertPostLinkAsync(PostLink post, TransactionConnection transaction)
    {
        MySqlCommand command = new(PostRepositoryCommands.SavePostLink);

        command.Parameters.AddWithValue("@id", post.Id);
        command.Parameters.AddWithValue("@url", post.Url);

        return await transaction.ExecuteInTransactionAsync(command);
    }

    private static async Task<int> InsertPostBaseAsync(Post post, TransactionConnection transaction)
    {
        MySqlCommand command = new(PostRepositoryCommands.SavePost);

        command.Parameters.AddWithValue("@id", post.Id);
        command.Parameters.AddWithValue("@community_id", post.CommunityId);
        command.Parameters.AddWithValue("@title", post.Title);
        command.Parameters.AddWithValue("@post_type", post.PostTypeValue);
        command.Parameters.AddWithValue("@author_id", post.AuthorId);
        command.Parameters.AddWithValue("@created_on", post.CreatedOn);
        command.Parameters.AddWithValue("@flair_post_id", post.FlairPostId);

        return await transaction.ExecuteInTransactionAsync(command);
    }


    public async Task<int> UpdatePostAsync(PostText post)
    {
        MySqlCommand command = new(PostRepositoryCommands.SavePostText);

        command.Parameters.AddWithValue("@id", post.Id);
        command.Parameters.AddWithValue("@content", post.Content);

        return await _connection.ModifyAsync(command);
    }

    #endregion

    #region - Delete -

    public async Task<int> MarkPostDeletedAsync(Guid postId)
    {
        MySqlCommand command = new(PostRepositoryCommands.AuthorDeletePost);

        command.Parameters.AddWithValue("@post_id", postId);

        return await _connection.ModifyAsync(command);
    }


    #endregion

    #region - Lock post -

    public async Task<int> UpdatePostLockedAsync(Guid postId, bool isLocked)
    {
        MySqlCommand command = new(PostRepositoryCommands.SetPostLocked);

        DateTime? lockedOn = isLocked ? DateTime.UtcNow : null;

        command.Parameters.AddWithValue("@locked_on", lockedOn);
        command.Parameters.AddWithValue("@post_id", postId);

        return await _connection.ModifyAsync(command);
    }


    #endregion



    public async Task<int> UpdatePostModerationFieldsAsync(Guid postId, ModeratePostForm form)
    {


        string removedOnClause = "mod_removed_on";

        if (form.Removed.HasValue)
        {
            removedOnClause = "@removed_on";
        }


        string lockedOnClause = "locked_on";

        if (form.Locked.HasValue)
        {
            lockedOnClause = "@locked_on";
        }

        string sql = string.Format(PostRepositoryCommands.UpdateModerationFields, removedOnClause, lockedOnClause);



        MySqlCommand command = new(sql);

        if (form.Locked is bool isLocked)
        {
            command.Parameters.AddWithValue("@locked_on", isLocked ? DateTime.UtcNow : null);
        }


        if (form.Removed is bool isRemoved)
        {
            command.Parameters.AddWithValue("@removed_on", isRemoved ? DateTime.UtcNow : null);
        }

        command.Parameters.AddWithValue("@post_id", postId);


        return await _connection.ModifyAsync(command);
    }

}
