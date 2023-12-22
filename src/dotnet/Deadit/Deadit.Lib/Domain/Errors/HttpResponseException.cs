using Deadit.Lib.Domain.Enum;
using System.Net;

namespace Deadit.Lib.Domain.Errors;

public class HttpResponseException : Exception
{
    public HttpResponseException(HttpStatusCode statusCode, object? value = null) => (StatusCode, Value) = ((int)statusCode, value);

    public int StatusCode { get; }

    public object? Value { get; }
}

public class ServiceErrorCodeException : Exception
{
    public ServiceErrorCodeException(HttpStatusCode statusCode, ErrorCode value) => (StatusCode, Value) = ((int)statusCode, value);

    public int StatusCode { get; }

    public ErrorCode Value { get; }
}


