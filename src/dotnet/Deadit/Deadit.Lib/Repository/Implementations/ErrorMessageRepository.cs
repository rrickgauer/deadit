﻿using System.Data;
using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Repository.Commands;
using Deadit.Lib.Repository.Contracts;
using Deadit.Lib.Repository.Other;
using MySql.Data.MySqlClient;

namespace Deadit.Lib.Repository.Implementations;


[AutoInject(AutoInjectionType.Singleton, InjectionProject.Always, InterfaceType = typeof(IErrorMessageRepository))]
public class ErrorMessageRepository : IErrorMessageRepository
{
    private readonly DatabaseConnection _dbConnection;

    public ErrorMessageRepository(DatabaseConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<DataTable> SelectErrorMessagesAsync()
    {
        MySqlCommand command = new(ErrorMessageRepositoryCommands.SelectAll);
        return await _dbConnection.FetchAllAsync(command);
    }
}
