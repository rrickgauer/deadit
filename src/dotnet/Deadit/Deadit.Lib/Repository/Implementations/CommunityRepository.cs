using System.Data;
using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Forms;
using Deadit.Lib.Domain.Model;
using Deadit.Lib.Repository.Commands;
using Deadit.Lib.Repository.Contracts;
using Deadit.Lib.Repository.Other;
using MySql.Data.MySqlClient;

namespace Deadit.Lib.Repository.Implementations;

[AutoInject<ICommunityRepository>(AutoInjectionType.Scoped, InjectionProject.Always)]
public class CommunityRepository(DatabaseConnection dbConnection) : ICommunityRepository
{
    private readonly DatabaseConnection _dbConnection = dbConnection;

    public async Task<DataRow?> SelectCommunityAsync(string communityName)
    {
        MySqlCommand command = new(CommunityRepositoryCommands.SelectCommunityByName);
        command.Parameters.AddWithValue("@community_name", communityName);

        return await _dbConnection.FetchAsync(command);
    }

    public async Task<DataRow?> SelectCommunityAsync(uint communityId)
    {
        MySqlCommand command = new(CommunityRepositoryCommands.SelectCommunityById);
        
        command.Parameters.AddWithValue("@id", communityId);

        return await _dbConnection.FetchAsync(command);
    }

    /// <summary>
    /// Insert the community into the database
    /// </summary>
    /// <param name="createCommunity"></param>
    /// <param name="userId"></param>
    /// <returns>The community id</returns>
    public async Task<uint?> InsertCommunityAsync(CreateCommunityRequestForm createCommunity, uint userId)
    {
        MySqlCommand command = new(CommunityRepositoryCommands.InsertCommunity);

        command.Parameters.AddWithValue("@name", createCommunity.Name);
        command.Parameters.AddWithValue("@title", createCommunity.Title);
        command.Parameters.AddWithValue("@description", createCommunity.Description);
        command.Parameters.AddWithValue("@owner_id", userId);

        return await _dbConnection.ModifyWithRowIdAsync(command);
    }

    public async Task<DataTable> SelectCreatedCommunitiesAsync(uint userId)
    {
        MySqlCommand command = new(CommunityRepositoryCommands.SelectUserCreatedCommunities);

        command.Parameters.AddWithValue("@user_id", userId);

        return await _dbConnection.FetchAllAsync(command);
    }


    public async Task<int> UpdateCommunityAsync(Community community)
    {
        MySqlCommand command = new(CommunityRepositoryCommands.UpdateCommunity);

        command.Parameters.AddWithValue("@title", community.Title);
        command.Parameters.AddWithValue("@description", community.Description);
        command.Parameters.AddWithValue("@community_type", community.CommunityType);
        command.Parameters.AddWithValue("@text_post_body_rule", community.TextPostBodyRule);
        command.Parameters.AddWithValue("@membership_closed_on", community.MembershipClosedOn);
        command.Parameters.AddWithValue("@flair_post_rule", community.FlairPostRule);
        command.Parameters.AddWithValue("@id", community.Id);

        return await _dbConnection.ModifyAsync(command);
    }
}
