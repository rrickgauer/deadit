using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Model;
using Deadit.Lib.Repository.Contracts;
using Deadit.Lib.Service.Contracts;

namespace Deadit.Lib.Service.Implementations;

[AutoInject<IErrorMessageService>(AutoInjectionType.Singleton, InjectionProject.Always)]
public class ErrorMessageService(IErrorMessageRepository repo, ITableMapperService tableMapperService) : IErrorMessageService
{
    private readonly IErrorMessageRepository _repo = repo;
    private readonly ITableMapperService _tableMapperService = tableMapperService;

    private static bool _messagesSet = false;

    private static readonly Dictionary<ErrorCode, ErrorMessage> _errorMessagesDict = new();

    public static Dictionary<ErrorCode, ErrorMessage> ErrorMessagesDict
    {
        get
        {
            if (!_messagesSet)
            {
                throw new Exception($"Error messages have not been loaded into memory yet!");
            }

            return _errorMessagesDict;
        }
    }

    public async Task LoadStaticErrorMessagesAsync()
    {
        if (!_messagesSet)
        {
            await LoadStaticErrorMessagesDictAsync();
        }
    }

    private async Task LoadStaticErrorMessagesDictAsync()
    {
        var table = await _repo.SelectErrorMessagesAsync();
        var messages = _tableMapperService.ToModels<ErrorMessage>(table);

        foreach (var message in messages)
        {
            var id = (ErrorCode)message;
            _errorMessagesDict.TryAdd(id, message);
        }

        _messagesSet = true;
    }


    public static List<ErrorMessage> ToErrorMessages(IEnumerable<ErrorCode> errorCodes)
    {
        List<ErrorMessage> messages = new();

        foreach (var errorCode in errorCodes)
        {
            var message = ErrorMessagesDict[errorCode];
            messages.Add(message);
        }

        return messages;
    }

}
