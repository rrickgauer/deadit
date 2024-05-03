/*******************************************************************************************

This filter checks if the user is logged in:
    - yes: continue on
    - no: redirect them to the login page, which will then redirect them to their final destination after log in

********************************************************************************************/

using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Service.Contracts;
using Deadit.WebGui.Controllers.Gui;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Deadit.WebGui.Filter;


[AutoInject(AutoInjectionType.Scoped, InjectionProject.WebGui)]
public class LoginFirstRedirectFilter(IAuthService authService) : IAsyncActionFilter
{
    private readonly IAuthService _authService = authService;

    private bool IsClientLoggedIn => _authService.IsClientLoggedIn();

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!IsClientLoggedIn)
        {
            context.Result = new RedirectToActionResult(nameof(LoginController.LoginPageAsync), LoginController.ControllerRedirectName, new
            {
                destination = context.HttpContext.Request.GetEncodedUrl(),
            });

            return;
        }

        await next();
    }
}
