using Deadit.Lib.Auth;
using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Utility;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Deadit.Lib.Filter;

public abstract class FlairPostFilterBase(FlairPostAuth auth) : IAsyncActionFilter
{
    protected abstract FlairPostAuthValidationLevel ValidationLevel { get; }
    
    protected readonly FlairPostAuth _auth = auth;

    public async virtual Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var hasPermission = await _auth.HasPermissionAsync(new()
        {
            ClientId = context.GetSessionClientId(),
            FlairForm = context.GetFlairPostForm(),
            FlairId = context.GetFlairPostIdRouteValueNull(),
            ValidationLevel = ValidationLevel,
        });

        if (!hasPermission.Successful)
        {
            context.ReturnBadServiceResponse(hasPermission);
            return;
        }

        await next();
    }
}



[AutoInject(AutoInjectionType.Scoped, InjectionProject.WebGui)]
public class PutFlairPostFilter(FlairPostAuth auth) : FlairPostFilterBase(auth)
{
    protected override FlairPostAuthValidationLevel ValidationLevel => FlairPostAuthValidationLevel.Update;
}


[AutoInject(AutoInjectionType.Scoped, InjectionProject.WebGui)]
public class PostFlairPostFilter(FlairPostAuth auth) : FlairPostFilterBase(auth)
{
    protected override FlairPostAuthValidationLevel ValidationLevel => FlairPostAuthValidationLevel.Insert;
}

[AutoInject(AutoInjectionType.Scoped, InjectionProject.WebGui)]
public class DeleteFlairPostFilter(FlairPostAuth auth) : FlairPostFilterBase(auth)
{
    protected override FlairPostAuthValidationLevel ValidationLevel => FlairPostAuthValidationLevel.Delete;
}


