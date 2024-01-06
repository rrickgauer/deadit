using Deadit.Lib.Domain.Constants;
using Deadit.Lib.Service.Contracts;
using Deadit.WebGui.Controllers.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Deadit.WebGui.Controllers.Gui;

[Controller]
[Route("")]
public class HomeController : Controller, IControllerName
{
    public static string ControllerRedirectName => IControllerName.RemoveControllerSuffix(nameof(HomeController));

    private readonly IAuthService _authService;

    public HomeController(IAuthService authService)
    {   
        _authService = authService; 
    }

    /// <summary>
    /// deadit.com
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> HomePageAsync()
    {
        return View(GuiPageViewFiles.HomePage); 
    }

    [HttpGet("/logout")]
    public async Task<IActionResult> LogoutPage()
    {
        // clear client session data
        _authService.ClearSessionData(HttpContext.Session);

        // return to the home page
        return LocalRedirect("/");
    }

}
