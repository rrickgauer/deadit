using Deadit.Lib.Auth;
using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Other;
using Deadit.Lib.Utility;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Deadit.Lib.Filter;

[AutoInject(AutoInjectionType.Scoped, InjectionProject.Always)]
public class PostExistsFilter(GetPostAuth auth) : IAsyncActionFilter
{
    private readonly GetPostAuth _auth = auth;

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var authResponse = await _auth.HasPermissionAsync(new()
        {
            PostId = context.GetPostIdRouteValue(),
        });

        if (!authResponse.Successful)
        {
            context.ReturnBadServiceResponse(authResponse);
            return;
        }

        await next();
    }
}
