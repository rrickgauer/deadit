using Deadit.Lib.Auth;
using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Errors;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace Deadit.WebGui.Filter;

[AutoInject(AutoInjectionType.Scoped, InjectionProject.WebGui)]
public class InternalApiAuthFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        SessionManager sessionManager = new(context.HttpContext.Session);

        if (!sessionManager.IsClientAuthorized())
        {
            throw new HttpResponseException(HttpStatusCode.Forbidden);
        }

        await next();
    }
}
