using System.Net;

namespace Deadit.Lib.Domain.Errors;

public class NotFoundHttpResponseException() : HttpResponseException(HttpStatusCode.NotFound, null)
{

}

