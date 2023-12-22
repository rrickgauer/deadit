using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadit.Lib.Service.Contracts;

public interface IErrorMessageService
{
    public Task<ErrorMessage> GetErrorMessageAsync(ErrorCode errorCode);
    public Task<ErrorMessage> GetErrorMessageAsync(int errorCode);
    public Task<Dictionary<ErrorCode, ErrorMessage>> GetErrorMessagesAsync();
}
