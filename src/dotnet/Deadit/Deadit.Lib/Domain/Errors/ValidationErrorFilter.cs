using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Deadit.Lib.Domain.Errors;

public class ValidationErrorFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.ModelState.IsValid)
        {
            ValidationFailureResponse response = new(context.ModelState);
            context.Result = new UnprocessableEntityObjectResult(response);

            return;
        }

        await next();
    }
}
