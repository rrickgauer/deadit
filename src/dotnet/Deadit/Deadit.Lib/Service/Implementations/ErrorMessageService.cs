﻿using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Model;
using Deadit.Lib.Repository.Contracts;
using Deadit.Lib.Service.Contracts;

namespace Deadit.Lib.Service.Implementations;

[AutoInject(AutoInjectionType.Singleton, InjectionProject.Always, InterfaceType = typeof(IErrorMessageService))]
public class ErrorMessageService : IErrorMessageService
{
    private static Dictionary<ErrorCode, ErrorMessage> _errorMessagesDict = new();
    private static bool _messagesSet = false;

    private readonly IErrorMessageRepository _repo;
    private readonly ITableMapperService _tableMapperService;

    public ErrorMessageService(IErrorMessageRepository repo, ITableMapperService tableMapperService)
    {
        _repo = repo;
        _tableMapperService = tableMapperService;
    }

    public async Task<Dictionary<ErrorCode, ErrorMessage>> GetErrorMessagesAsync()
    {
        if (!_messagesSet)
        {
            await LoadStaticErrorMessagesDictAsync();
        }

        return _errorMessagesDict;
    }

    private async Task LoadStaticErrorMessagesDictAsync()
    {
        var table = await _repo.SelectErrorMessagesAsync();
        var messages = _tableMapperService.ToModels<ErrorMessage>(table);

        foreach (var message in messages)
        {
            var id = (ErrorCode)message.Id.Value;
            _errorMessagesDict.TryAdd(id, message);
        }

        _messagesSet = true;
    }
}
