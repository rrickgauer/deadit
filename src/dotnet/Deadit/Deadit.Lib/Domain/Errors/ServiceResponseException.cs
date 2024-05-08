using Deadit.Lib.Domain.Response;

namespace Deadit.Lib.Domain.Errors;

public class ServiceResponseException(ServiceResponse serviceResponse) : Exception()
{
    public ServiceResponse Response { get; } = serviceResponse;
}
