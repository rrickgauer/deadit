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

    public async Task<DataRow?> SelectUserAsync(uint userId)
    {
        MySqlCommand command = new(UserRepositoryCommands.SelectUserById);

        command.Parameters.AddWithValue("@user_id", userId);

        return await _dbConnection.FetchAsync(command);
    }

    public async Task<DataTable> SelectMatchingUsersAsync(string email, string username)
    {
        MySqlCommand command = new(UserRepositoryCommands.SelectByEmailOrUsername);

        command.Parameters.AddWithValue("@email", email);
        command.Parameters.AddWithValue("@username", username);

        return await _dbConnection.FetchAllAsync(command);
    }

    public async Task<uint?> InsertAsync(SignupRequestForm signupForm)
    {
        MySqlCommand command = new(UserRepositoryCommands.InsertUser);

        command.Parameters.AddWithValue("@email", signupForm.Email);
        command.Parameters.AddWithValue("@username", signupForm.Username);
        command.Parameters.AddWithValue("@password", signupForm.Password);

        var userId = await _dbConnection.ModifyWithRowIdAsync(command);

        return userId;
    }
}
