using Deadit.Lib.Domain.Forms;
using Deadit.Lib.Repository.Commands;
using Deadit.Lib.Repository.Contracts;
using Deadit.Lib.Repository.Other;
using MySql.Data.MySqlClient;
using System.Data;

namespace Deadit.Lib.Repository.Implementations;

public class UserRepository : IUserRepository
{
    private readonly DatabaseConnection _dbConnection;

    public UserRepository(DatabaseConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }


    public async Task<DataRow?> SelectUserAsync(LoginRequestForm loginForm)
    {
        MySqlCommand command = new(UserRepositoryCommands.SelectUserByLoginInfo);

        command.Parameters.AddWithValue("@username", loginForm.Username);
        command.Parameters.AddWithValue("@password", loginForm.Password);

        return await _dbConnection.FetchAsync(command);
    }
}
