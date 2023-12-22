using Deadit.Lib.Domain.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadit.Lib.Service.Contracts;

public interface IResponseService
{
    public Task<ApiResponse<object>> ToApiResponseAsync(ServiceResponse response);
    public Task<ApiResponse<T>> ToApiResponseAsync<T>(ServiceDataResponse<T> response);
}
