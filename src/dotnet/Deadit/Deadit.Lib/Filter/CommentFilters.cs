using Deadit.Lib.Auth;
using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Deadit.Lib.Filter;

public class CommentFilters
{
    [AutoInject(AutoInjectionType.Scoped, InjectionProject.Always)]
    public class CommentSaveFilter(CommentAuth auth) : IAsyncActionFilter
    {
        private readonly CommentAuth _auth = auth;

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var authResponse = await _auth.HasPermissionAsync(new()
            {
                CommentId = context.GetCommentIdRouteValue(),
                UserId = context.GetSessionClientId(),
                IsDelete = false,
            });

            if (!authResponse.Successful)
            {
                context.Result = new BadRequestObjectResult(authResponse);
                return;
            }

            await next();
        }
    }


    [AutoInject(AutoInjectionType.Scoped, InjectionProject.Always)]
    public class CommentDeleteFilter(CommentAuth auth) : IAsyncActionFilter
    {
        private readonly CommentAuth _auth = auth;

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var authResponse = await _auth.HasPermissionAsync(new()
            {
                CommentId = context.GetCommentIdRouteValue(),
                UserId = context.GetSessionClientId(),
                IsDelete = true,
            });

            if (!authResponse.Successful)
            {
                context.Result = new BadRequestObjectResult(authResponse);
                return;
            }

            await next();
        }
    }


}
