using System.Net;

namespace Deadit.Lib.Domain.HttpResponses;

public class HttpResponseException : Exception
{
    public HttpResponseException(HttpStatusCode statusCode, object? value = null) => (StatusCode, Value) = ((int)statusCode, value);

    public int StatusCode { get; }

    public object? Value { get; }
}
