﻿using System.Data;
using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Constants;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Model;
using Deadit.Lib.Domain.Other;
using Deadit.Lib.Domain.Paging;
using Deadit.Lib.Repository.Commands;
using Deadit.Lib.Repository.Contracts;
using Deadit.Lib.Repository.Other;
using MySql.Data.MySqlClient;

namespace Deadit.Lib.Repository.Implementations;

[AutoInject<ICommunityMembershipRepository>(AutoInjectionType.Scoped, InjectionProject.Always)]
public class CommunityMembershipRepository(DatabaseConnection connection) : ICommunityMembershipRepository
{
    private readonly DatabaseConnection _dbConnection = connection;

    public async Task<DataTable> SelectUserJoinedCommunitiesAsync(uint userId)
    {
        MySqlCommand command = new(CommunityMembershipRepositoryCommands.SelectAllByUserId);
        command.Parameters.AddWithValue("@user_id", userId);

        return await _dbConnection.FetchAllAsync(command);
    }

    public async Task<DataRow?> SelectCommunityMembershipAsync(uint userId, uint communityId)
    {
        MySqlCommand command = new(CommunityMembershipRepositoryCommands.SelectByUserIdCommunityId);

        command.Parameters.AddWithValue("@user_id", userId);
        command.Parameters.AddWithValue("@community_id", communityId);

        return await _dbConnection.FetchAsync(command);
    }

    public async Task<DataRow?> SelectCommunityMembershipAsync(uint userId, string communityName)
    {
        MySqlCommand command = new(CommunityMembershipRepositoryCommands.SelectByUserIdCommunityName);

        command.Parameters.AddWithValue("@user_id", userId);
        command.Parameters.AddWithValue("@community_name", communityName);

        return await _dbConnection.FetchAsync(command);
    }

    public async Task<DataRow?> SelectCommunityMembershipAsync(string username, string communityName)
    {
        MySqlCommand command = new(CommunityMembershipRepositoryCommands.SelectByUsernameCommunityName);

        command.Parameters.AddWithValue("@username", username);
        command.Parameters.AddWithValue("@community_name", communityName);

        return await _dbConnection.FetchAsync(command);
    }

    /// <summary>
    /// Delete the membership record
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="communityName"></param>
    /// <returns>Number of affected records</returns>
    public async Task<int> DeleteMembershipAsync(uint userId, string communityName)
    {
        MySqlCommand command = new(CommunityMembershipRepositoryCommands.DeleteByUserIdAndCommunityName);

        command.Parameters.AddWithValue("@user_id", userId);
        command.Parameters.AddWithValue("@community_name", communityName);

        return await _dbConnection.ModifyAsync(command);
    }

    public async Task<int> InsertMembershipAsync(uint userId, string communityName)
    {
        MySqlCommand command = new(CommunityMembershipRepositoryCommands.InsertByUserIdAndCommunityName);

        command.Parameters.AddWithValue("@user_id", userId);
        command.Parameters.AddWithValue("@community_name", communityName);

        return await _dbConnection.ModifyAsync(command);
    }


    public async Task<DataTable> SelectCommunityMembersAsync(string communityName)
    {
        MySqlCommand command = new(CommunityMembershipRepositoryCommands.SelectAllCommunityMembers);

        command.Parameters.AddWithValue("@community_name", communityName);

        return await _dbConnection.FetchAllAsync(command);
    }

    public async Task<DataTable> SelectCommunityMembersAsync(string communityName, CommunityMembersSorting sorting, PaginationCommunityMembers pagination)
    {
        string sql = string.Format(CommunityMembershipRepositoryCommands.SelectCommunityMembersSortedTemplate, sorting.GetSqlOrderClause(), RepositoryConstants.PAGINATION_CLAUSE);
        MySqlCommand command = new(sql);

        command.Parameters.AddWithValue("@community_name", communityName);
        command.AddPaginationParamters(pagination);

        return await _dbConnection.FetchAllAsync(command);
    }


}
