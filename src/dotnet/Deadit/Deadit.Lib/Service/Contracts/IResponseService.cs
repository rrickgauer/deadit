using Deadit.Lib.Domain.Errors;

namespace Deadit.Lib.Service.Contracts;

public interface IResponseService
{
    public Task<ApiResponse<object>> ToApiResponseAsync(ServiceResponse response);
    public Task<ApiResponse<T>> ToApiResponseAsync<T>(ServiceDataResponse<T> response);
    public ApiResponse<object> GetEmptyApiResponse();
}
