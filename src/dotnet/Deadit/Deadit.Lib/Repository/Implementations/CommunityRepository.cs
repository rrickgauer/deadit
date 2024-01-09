using System.Data;
using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Forms;
using Deadit.Lib.Repository.Commands;
using Deadit.Lib.Repository.Contracts;
using Deadit.Lib.Repository.Other;
using MySql.Data.MySqlClient;

namespace Deadit.Lib.Repository.Implementations;

[AutoInject(AutoInjectionType.Scoped, InjectionProject.Always, InterfaceType = typeof(ICommunityRepository))]
public class CommunityRepository : ICommunityRepository
{
    private readonly DatabaseConnection _dbConnection;

    public CommunityRepository(DatabaseConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

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
}
