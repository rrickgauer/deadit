using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Errors;
using Deadit.Lib.Domain.Model;
using Deadit.Lib.Service.Contracts;

namespace Deadit.Lib.Service.Implementations;

public class ResponseService : IResponseService
{
    private readonly IErrorMessageService _errorMessageService;

    private async Task<Dictionary<ErrorCode, ErrorMessage>> GetErrorsDictAsync() => await _errorMessageService.GetErrorMessagesAsync();

    public ResponseService(IErrorMessageService errorMessageService)
    {
        _errorMessageService = errorMessageService;
    }

    public async Task<ApiResponse<object>> ToApiResponseAsync(ServiceResponse response)
    {
        var messages = await GetErrorMessagesAsync(response.Errors);

        ApiResponse<object> result = new()
        {
            Data = null,
            Errors = messages
        };

        return result;
    }

    public async Task<ApiResponse<T>> ToApiResponseAsync<T>(ServiceDataResponse<T> response)
    {
        var messages = await GetErrorMessagesAsync(response.Errors);

        ApiResponse<T> result = new()
        {
            Data = response.Data,
            Errors = messages
        };

        return result;
    }


    private async Task<List<ErrorMessage>> GetErrorMessagesAsync(IEnumerable<ErrorCode> errorCodes)
    {
        var errorsReference = await GetErrorsDictAsync();

        List<ErrorMessage> messages = new();

        foreach (var errorCode in errorCodes)
        {
            var message = errorsReference[errorCode];
            messages.Add(message);
        }

        return messages;
    }

    
}
