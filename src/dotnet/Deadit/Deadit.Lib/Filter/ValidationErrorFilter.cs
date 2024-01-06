using Deadit.Lib.Domain.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Deadit.Lib.Filter;

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
