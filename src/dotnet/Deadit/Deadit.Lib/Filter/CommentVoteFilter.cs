using Deadit.Lib.Auth;
using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Errors;
using Deadit.Lib.Utility;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Deadit.Lib.Filter;

[AutoInject(AutoInjectionType.Scoped, InjectionProject.Always)]
public class CommentVoteFilter(CommentVoteAuth auth) : IAsyncActionFilter
{
    private readonly CommentVoteAuth _auth = auth;

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        try
        {

            var result = await _auth.HasPermissionAsync(new()
            {
                CommentId = context.GetCommentIdRouteValue(),
            });

            if (!result.Successful)
            {
                context.ReturnBadServiceResponse(result);
                return;
            }

            await next();
        }
        catch (ServiceResponseException ex)
        {
            context.ReturnBadServiceResponse(ex.Response);
            return;
        }
    }
}
