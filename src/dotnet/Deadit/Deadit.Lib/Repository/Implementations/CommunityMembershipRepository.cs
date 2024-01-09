using System.Data;
using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Repository.Commands;
using Deadit.Lib.Repository.Contracts;
using Deadit.Lib.Repository.Other;
using MySql.Data.MySqlClient;

namespace Deadit.Lib.Repository.Implementations;

[AutoInject(AutoInjectionType.Scoped, InjectionProject.Always, InterfaceType = typeof(ICommunityMembershipRepository))]
public class CommunityMembershipRepository : ICommunityMembershipRepository
{
    private readonly DatabaseConnection _dbConnection;

    public CommunityMembershipRepository(DatabaseConnection connection)
    {
        _dbConnection = connection;
    }

    public async Task<DataTable> SelectUserJoinedCommunitiesAsync(uint userId)
    {
        MySqlCommand command = new(CommunityMembershipRepositoryCommands.SelectAllByUserId);
        command.Parameters.AddWithValue("@user_id", userId);

        return await _dbConnection.FetchAllAsync(command);
    }
}
