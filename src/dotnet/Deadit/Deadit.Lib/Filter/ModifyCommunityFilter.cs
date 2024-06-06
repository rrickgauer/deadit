using Deadit.Lib.Auth;
using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Utility;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Deadit.Lib.Filter;

[AutoInject(AutoInjectionType.Scoped, InjectionProject.WebGui)]
public class ModifyCommunityFilter(ModifyCommunityAuth auth) : IAsyncActionFilter
{
    private readonly ModifyCommunityAuth _auth = auth;

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var permission = await _auth.HasPermissionAsync(new()
        {
            ClientId = context.GetSessionClientId(),
            CommunityName = context.GetCommunityNameRouteValue(),
        });

        if (!permission.Successful)
        {
            context.ReturnBadServiceResponse(permission);
            return;
        }


        await next();
    }
}
