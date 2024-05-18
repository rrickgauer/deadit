using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Configurations;
using Deadit.Lib.Domain.Constants;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Errors;
using MySql.Data.MySqlClient;

namespace Deadit.Lib.Repository.Other;

[AutoInject(AutoInjectionType.Transient, InjectionProject.Always)]
public class TransactionConnection(IConfigs configs) : DatabaseConnection(configs)
{
    protected MySqlConnection _connection = new();
    protected MySqlTransaction? _transaction;

    public async Task StartTransactionAsync()
    {
        // setup a new database connection object
        _connection= GetNewConnection();
        await _connection.OpenAsync();

        // start up a transaction
        _transaction = await _connection.BeginTransactionAsync();
    }

    public async Task<int> ExecuteInTransactionAsync(MySqlCommand command)
    {
        CheckConnection();

        try
        {
            command.Connection = _connection;
            return await command.ExecuteNonQueryAsync();
        }

        catch (MySqlException ex)
        {
            await _transaction!.RollbackAsync();

            if (ex.Number == RepositoryConstants.USER_DEFINED_EXCEPTION_NUMBER)
            {
                throw new RepositoryException(ex);
            }

            throw;
        }
    }

    public async Task CommitAsync()
    {
        CheckConnection();

        await _transaction!.CommitAsync();
        await CloseConnectionAsync(_connection);
    }

    private void CheckConnection()
    {
        if (_transaction == null)
        {
            throw new Exception($"Transaction has not been initialized yet.");
        }
    }



}
