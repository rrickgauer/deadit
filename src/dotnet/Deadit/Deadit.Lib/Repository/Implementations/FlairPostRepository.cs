using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Model;
using Deadit.Lib.Repository.Commands;
using Deadit.Lib.Repository.Contracts;
using Deadit.Lib.Repository.Other;
using MySql.Data.MySqlClient;
using System.Data;

namespace Deadit.Lib.Repository.Implementations;


[AutoInject<IFlairPostRepository>(AutoInjectionType.Scoped, InjectionProject.Always)]
public class FlairPostRepository(DatabaseConnection connection) : IFlairPostRepository
{
    private readonly DatabaseConnection _connection = connection;

    public async Task<DataTable> SelectCommunityPostFlairsAsync(uint communityId)
    {
        MySqlCommand command = new(FlairPostRepositoryCommands.SelectAllByCommunityId);

        command.Parameters.AddWithValue("@community_id", communityId);

        return await _connection.FetchAllAsync(command);
    }

    public async Task<DataTable> SelectCommunityPostFlairsAsync(string communityName)
    {
        MySqlCommand command = new(FlairPostRepositoryCommands.SelectAllByCommunityName);

        command.Parameters.AddWithValue("@community_name", communityName);

        return await _connection.FetchAllAsync(command);
    }

    public async Task<DataRow?> SelectPostFlairAsync(uint flairId)
    {
        MySqlCommand command = new(FlairPostRepositoryCommands.SelectById);

        command.Parameters.AddWithValue("@flair_id", flairId);

        return await _connection.FetchAsync(command);
    }

    public async Task<int> UpdateFlairAsync(FlairPost flair)
    {
        MySqlCommand command = new(FlairPostRepositoryCommands.Update);

        command.Parameters.AddWithValue("@id", flair.Id);
        command.Parameters.AddWithValue("@name", flair.Name);
        command.Parameters.AddWithValue("@color", flair.Color);

        return await _connection.ModifyAsync(command);
    }
    public async Task<uint?> InsertFlairAsync(FlairPost flair)
    {
        MySqlCommand command = new(FlairPostRepositoryCommands.Insert);

        command.Parameters.AddWithValue("@id", flair.Id);
        command.Parameters.AddWithValue("@community_id", flair.CommunityId);
        command.Parameters.AddWithValue("@name", flair.Name);
        command.Parameters.AddWithValue("@color", flair.Color);
        command.Parameters.AddWithValue("@created_on", flair.CreatedOn);

        return await _connection.ModifyWithRowIdAsync(command);
    }


    public async Task<int> DeleteFlairAsync(uint flairId)
    {
        MySqlCommand command = new(FlairPostRepositoryCommands.Delete);

        command.Parameters.AddWithValue("@id", flairId);

        return await _connection.ModifyAsync(command);
    }
}
