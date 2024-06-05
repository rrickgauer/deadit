using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Response;

namespace Deadit.Lib.Domain.Errors;

public class ServiceResponseException(ServiceResponse serviceResponse) : Exception()
{
    public ServiceResponseException(ErrorCode errorCode) : this(new ServiceResponse(errorCode)) { }

    public ServiceResponse Response { get; } = serviceResponse;
}
