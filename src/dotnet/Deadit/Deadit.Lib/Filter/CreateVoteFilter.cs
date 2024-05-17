using Deadit.Lib.Auth;
using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Errors;
using Deadit.Lib.Utility;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Deadit.Lib.Filter;

[AutoInject(AutoInjectionType.Scoped, InjectionProject.Always)]
public class CreateVoteFilter(VoteAuth voteAuth) : IAsyncActionFilter
{
    private readonly VoteAuth _voteAuth = voteAuth;

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        try
        {
            var isAuth = await _voteAuth.HasPermissionAsync(new()
            {
                VoteForm = context.GetRequestCreateVoteForm(),
                ClientId = context.GetSessionClientId(),
            });

            if (!isAuth.Successful)
            {
                context.ReturnBadServiceResponse(isAuth);
                return;
            }
        }
        catch(ServiceResponseException ex)
        {
            context.ReturnBadServiceResponse(ex);
            return;
        }

        await next();
    }
}
