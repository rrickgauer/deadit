using Deadit.Lib.Domain.Enum;

namespace Deadit.Lib.Domain.Errors;

public class ErrorCodeException(ErrorCode errorCode) : Exception()
{
    public ErrorCode ErrorCode { get; } = errorCode;
}
