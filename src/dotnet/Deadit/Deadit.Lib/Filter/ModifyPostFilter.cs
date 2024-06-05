using Deadit.Lib.Auth;
using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Errors;
using Deadit.Lib.Utility;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Deadit.Lib.Filter;

[AutoInject(AutoInjectionType.Scoped, InjectionProject.Always)]
public class ModifyPostFilter(ModifyPostAuth auth) : IAsyncActionFilter
{
    private readonly ModifyPostAuth _auth = auth;

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        try
        {
            var authResponse = await _auth.HasPermissionAsync(new()
            {
                ClientId = context.GetSessionClientId(),
                PostId = context.GetPostIdRouteValue(),
            });

            if (!authResponse.Successful)
            {
                context.ReturnBadServiceResponse(authResponse);
                return;
            }

            await next();
        }
        catch(ServiceResponseException ex)
        {
            context.ReturnBadServiceResponse(ex);
            return;
        }
    }
}




