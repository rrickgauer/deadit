using System.Net;

namespace Deadit.Lib.Domain.Errors;

public class ForbiddenHttpResponseException() : HttpResponseException(HttpStatusCode.Forbidden, null)
{

}

