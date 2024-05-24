using Deadit.Lib.Auth;
using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Utility;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Deadit.Lib.Filter;

[AutoInject(AutoInjectionType.Scoped, InjectionProject.WebGui)]
public class ModerateCommentFilter(ModerateCommentAuth auth) : IAsyncActionFilter
{

    private readonly ModerateCommentAuth _auth = auth;

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var hasPermission = await _auth.HasPermissionAsync(new()
        {
            ClientId = context.GetSessionClientId(),
            CommentId = context.GetCommentIdRouteValue(),
            CommunityName = context.GetCommunityNameRouteValue(),
            PostId = context.GetPostIdRouteValue(),
        });

        if (!hasPermission.Successful)
        {
            context.ReturnBadServiceResponse(hasPermission);
            return;
        }


        await next();
    }
}
