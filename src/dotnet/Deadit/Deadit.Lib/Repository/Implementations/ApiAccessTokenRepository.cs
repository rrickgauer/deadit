using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Repository.Commands;
using Deadit.Lib.Repository.Contracts;
using Deadit.Lib.Repository.Other;
using MySql.Data.MySqlClient;
using System.Data;

namespace Deadit.Lib.Repository.Implementations;


[AutoInject<IApiAccessTokenRepository>(AutoInjectionType.Scoped, InjectionProject.Always)]
public class ApiAccessTokenRepository(DatabaseConnection connection) : IApiAccessTokenRepository
{
    private readonly DatabaseConnection _connection = connection;

    public async Task<DataRow?> SelectTokenAsync(Guid token)
    {
        MySqlCommand command = new(ApiAccessTokenRepositoryCommands.SelectToken);

        command.Parameters.AddWithValue("@token", token);

        return await _connection.FetchAsync(command);
    }
}
