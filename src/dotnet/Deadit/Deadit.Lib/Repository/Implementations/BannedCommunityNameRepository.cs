using System.Data;
using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Repository.Commands;
using Deadit.Lib.Repository.Contracts;
using Deadit.Lib.Repository.Other;
using MySql.Data.MySqlClient;

namespace Deadit.Lib.Repository.Implementations;

[AutoInject(AutoInjectionType.Scoped, InjectionProject.Always, InterfaceType = typeof(IBannedCommunityNameRepository))]
public class BannedCommunityNameRepository : IBannedCommunityNameRepository
{
    private readonly DatabaseConnection _dbConnection;

    public BannedCommunityNameRepository(DatabaseConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<DataTable> SelectAllBannedCommunityNamesAsync()
    {
        MySqlCommand command = new(BannedCommunityNameRepositoryCommands.SelectAll);
        return await _dbConnection.FetchAllAsync(command);
    }
}
