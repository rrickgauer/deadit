using System.Net;

namespace Deadit.Lib.Domain.Errors;

public class NotFoundHttpResponseException() : HttpResponseException(HttpStatusCode.NotFound, null)
{


    public static T ThrowIfNot<T>(object? data)
    {
        if (data is not T result)
        {
            throw new NotFoundHttpResponseException();
        }

        return result;
    }

}

