using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Repository.Commands;
using Deadit.Lib.Repository.Contracts;
using Deadit.Lib.Repository.Other;
using MySql.Data.MySqlClient;
using System.Data;
using static Deadit.Lib.Domain.Model.Votes;

namespace Deadit.Lib.Repository.Implementations;



[AutoInject<IPostVotesRepository>(AutoInjectionType.Scoped, InjectionProject.Always)]
public class PostVotesRepository(DatabaseConnection connection) : IPostVotesRepository
{
    private readonly DatabaseConnection _connection = connection;

    private static readonly PostVotesRepositoryCommands _commands = new();

    public async Task<int> SaveVoteAsync(VotePost vote)
    {
        MySqlCommand command = new(PostVotesRepositoryCommands.Save);

        command.Parameters.AddWithValue("@post_id", vote.ItemId);
        command.Parameters.AddWithValue("@user_id", vote.UserId);
        command.Parameters.AddWithValue("@vote_type", vote.VoteType);
        command.Parameters.AddWithValue("@created_on", vote.CreatedOn);

        return await _connection.ModifyAsync(command);
    }


    
    
    public async Task<DataTable> SelectPostVotesAsync(Guid postId)
    {
        MySqlCommand command = new(PostVotesRepositoryCommands.SelectAllPostVotes);

        command.Parameters.AddWithValue("@post_id", postId);

        return await _connection.FetchAllAsync(command);
    }
    
    
    public async Task<DataRow?> SelectVoteAsync(Guid postId, uint userId)
    {
        MySqlCommand command = new(PostVotesRepositoryCommands.SelectVote);

        command.Parameters.AddWithValue("@post_id", postId);
        command.Parameters.AddWithValue("@user_id", userId);

        return await _connection.FetchAsync(command);
    }


    public async Task<DataTable> SelectUserPostVotesInCommunityAsync(uint userId, string communityName)
    {
        MySqlCommand command = new(PostVotesRepositoryCommands.SelectAllUserVotesInCommunity);

        command.Parameters.AddWithValue("@user_id", userId);
        command.Parameters.AddWithValue("@community_name", communityName);

        return await _connection.FetchAllAsync(command);
    }

    public async Task<DataTable> SelectUserPostVotesAsync(uint userId, IEnumerable<Guid> postIds)
    {
        InClause<Guid> inClause = new(postIds);

        string sql = string.Format(PostVotesRepositoryCommands.SelectUserPostVotesLimit, inClause.GetSqlClause());

        MySqlCommand command = new(sql);

        command.Parameters.AddWithValue("@user_id", userId);
        
        inClause.AddParms(command);

        return await _connection.FetchAllAsync(command);
    }
}
