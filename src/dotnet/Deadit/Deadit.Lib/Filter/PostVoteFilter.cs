using Deadit.Lib.Auth;
using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Errors;
using Deadit.Lib.Utility;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Deadit.Lib.Filter;


[AutoInject(AutoInjectionType.Scoped, InjectionProject.Always)]
public class PostVoteFilter(PostVoteAuth auth) : IAsyncActionFilter
{
    private readonly PostVoteAuth _auth = auth;

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        
        try
        {

            var result = await _auth.HasPermissionAsync(new()
            {
                PostId = context.GetPostIdRouteValue(),
            });

            if (!result.Successful)
            {
                context.ReturnBadServiceResponse(result);
                return;
            }

            await next();
        }
        catch(ServiceResponseException ex)
        {
            context.ReturnBadServiceResponse(ex.Response);
            return;
        }
    }
}
