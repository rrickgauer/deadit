﻿using System.ComponentModel.Design;
using System.Data;
using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Model;
using Deadit.Lib.Domain.TableView;
using Deadit.Lib.Repository.Commands;
using Deadit.Lib.Repository.Contracts;
using Deadit.Lib.Repository.Other;
using MySql.Data.MySqlClient;

namespace Deadit.Lib.Repository.Implementations;


[AutoInject<ICommentRepository>(AutoInjectionType.Scoped, InjectionProject.Always)]
public class CommentRepository(DatabaseConnection connection) : ICommentRepository
{
    private readonly DatabaseConnection _connection = connection;

    public async Task<DataTable> SelectAllPostCommentsAsync(Guid postId, SortOption sortBy)
    {
        string sql = string.Format(CommentRepositoryCommands.SelectAllPostCommentsSortedTemplate, sortBy.GetOrderByClause<ViewComment>());

        MySqlCommand command = new(sql);

        command.Parameters.AddWithValue(@"post_id", postId);

        return await _connection.FetchAllAsync(command);
    }


    public async Task<int> SaveCommentAsync(Comment comment)
    {
        MySqlCommand command = new(CommentRepositoryCommands.Save);

        command.Parameters.AddWithValue("@id", comment.Id);
        command.Parameters.AddWithValue("@author_id", comment.AuthorId);
        command.Parameters.AddWithValue("@post_id", comment.PostId);
        command.Parameters.AddWithValue("@content", comment.Content);
        command.Parameters.AddWithValue("@parent_id", comment.ParentId);
        command.Parameters.AddWithValue("@created_on", comment.CreatedOn);

        return await _connection.ModifyAsync(command);
    }

    public async Task<DataRow?> SelectCommentAsync(Guid commentId)
    {
        MySqlCommand command = new(CommentRepositoryCommands.SelectComment);

        command.Parameters.AddWithValue(@"comment_id", commentId);

        return await _connection.FetchAsync(command);
    }

    public async Task<int> DeleteCommentAsync(Guid commentId)
    {
        MySqlCommand command = new(CommentRepositoryCommands.SetDeleted);

        command.Parameters.AddWithValue(@"comment_id", commentId);

        return await _connection.ModifyAsync(command);
    }

    public async Task<int> SaveCommentModerateAsync(Comment comment)
    {
        MySqlCommand command = new(CommentRepositoryCommands.SaveModerate);

        command.Parameters.AddWithValue(@"comment_id", comment.Id);
        command.Parameters.AddWithValue(@"locked_on", comment.LockedOn);
        command.Parameters.AddWithValue(@"removed_on", comment.RemovedOn);

        return await _connection.ModifyAsync(command);
    }
}