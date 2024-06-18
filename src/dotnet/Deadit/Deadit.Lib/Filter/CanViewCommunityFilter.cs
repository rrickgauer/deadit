using Deadit.Lib.Auth;
using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Constants;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Deadit.Lib.Filter;


[AutoInject(AutoInjectionType.Scoped, InjectionProject.WebGui)]
public class CanViewCommunityFilter(CanViewCommunityAuth auth) : IAsyncActionFilter
{

    private readonly CanViewCommunityAuth _auth = auth;

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var hasPermission = await _auth.HasPermissionAsync(new()
        {
            CommunityName = context.GetCommunityNameRouteValue(),
            ClientId = context.GetSessionsClientIdNull(),
        });

        if (hasPermission.Successful)
        {
            await next();
            return;
        }

        if (hasPermission.Exists(ErrorCode.CommunitySettingsPrivateCommunityAccessAttempt))
        {
            context.Result = new ViewResult()
            {
                ViewName = GuiPageViewFiles.CommunityIsPrivatePage,
            };
        }
        else
        {
            context.ReturnBadServiceResponse(hasPermission);
        }

        return;
    }
}
