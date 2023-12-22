using Deadit.Lib.Service.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Deadit.WebGui.Controllers.Gui;

[Controller]
[Route("")]
public class HomeController : Controller
{
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
        return View("HomePage");
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
