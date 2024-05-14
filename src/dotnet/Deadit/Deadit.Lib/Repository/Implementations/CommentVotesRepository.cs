using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Repository.Commands;
using Deadit.Lib.Repository.Contracts;
using Deadit.Lib.Repository.Other;
using MySql.Data.MySqlClient;
using System.Data;
using static Deadit.Lib.Domain.Model.Votes;

namespace Deadit.Lib.Repository.Implementations;


[AutoInject<ICommentVotesRepository>(AutoInjectionType.Scoped, InjectionProject.Always)]
public class CommentVotesRepository(DatabaseConnection connection) : ICommentVotesRepository
{
    private readonly DatabaseConnection _connection = connection;

    public async Task<int> SaveVoteAsync(VoteComment vote)
    {
        MySqlCommand command = new(CommentVotesRepositoryCommands.SaveVote);

        command.Parameters.AddWithValue("@comment_id", vote.ItemId);
        command.Parameters.AddWithValue("@user_id", vote.UserId);
        command.Parameters.AddWithValue("@vote_type", (ushort?)vote.VoteType);
        command.Parameters.AddWithValue("@created_on", vote.CreatedOn);

        return await _connection.ModifyAsync(command);
    }

    public async Task<DataTable> SelectCommentVotesAsync(Guid commentId)
    {
        MySqlCommand command = new(CommentVotesRepositoryCommands.SelectCommentVotes);

        command.Parameters.AddWithValue("@comment_id", commentId);

        return await _connection.FetchAllAsync(command);
    }

    public async Task<DataTable> SelectUserVotesInPost(Guid postId, uint userId)
    {
        MySqlCommand command = new(CommentVotesRepositoryCommands.SelectUserCommentVotesInPost);

        command.Parameters.AddWithValue("@post_id", postId);
        command.Parameters.AddWithValue("@user_id", userId);

        return await _connection.FetchAllAsync(command);
    }

    public async Task<DataRow?> SelectVoteAsync(Guid commentId, uint userId)
    {
        MySqlCommand command = new(CommentVotesRepositoryCommands.SelectVote);

        command.Parameters.AddWithValue("@comment_id", commentId);
        command.Parameters.AddWithValue("@user_id", userId);

        return await _connection.FetchAsync(command);
    }
}