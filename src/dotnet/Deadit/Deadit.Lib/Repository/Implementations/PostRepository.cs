using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Model;
using Deadit.Lib.Domain.Paging;
using Deadit.Lib.Repository.Commands;
using Deadit.Lib.Repository.Contracts;
using Deadit.Lib.Repository.Other;
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

    public async Task<DataTable> SelectNewestCommunityPostsAsync(string communityName, PaginationPosts pagination)
    {
        MySqlCommand command = new(PostRepositoryCommands.SelectNewestCommunityPostsLimit);

        command.Parameters.AddWithValue("@community_name", communityName);

        command.AddPaginationParamters(pagination);

        return await _connection.FetchAllAsync(command);
    }

    public async Task<DataTable> SelectNewestCommunityPostsAsync(string communityName)
    {
        return await RunSelectAllCommandAsync(communityName, PostRepositoryCommands.SelectNewestCommunityPosts);
    }

    public async Task<DataTable> SelectCommunityTextPostsAsync(string communityName)
    {
        return await RunSelectAllCommandAsync(communityName, PostRepositoryCommands.SelectAllCommunityText);
    }

    public async Task<DataTable> SelectCommunityLinkPostsAsync(string communityName)
    {
        return await RunSelectAllCommandAsync(communityName, PostRepositoryCommands.SelectAllCommunityLink);
    }

    private async Task<DataTable> RunSelectAllCommandAsync(string communityName, string sql)
    {
        MySqlCommand command = new(sql);

        command.Parameters.AddWithValue("@community_name", communityName);

        return await _connection.FetchAllAsync(command);
    }

    #endregion


    #region - Select Top Community Posts -

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

    private async Task<DataTable> SelectTopCommunityPostsAsync(MySqlCommand command, string communityName, DateTime createdAfter, PaginationPosts? pagination=null)
    {
        command.Parameters.AddWithValue("@community_name", communityName);
        command.Parameters.AddWithValue("@created_on", createdAfter);

        if (pagination != null)
        {
            command.AddPaginationParamters(pagination);
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

        return await transaction.ExecuteInTransactionAsync(command);
    }

    #endregion
}
