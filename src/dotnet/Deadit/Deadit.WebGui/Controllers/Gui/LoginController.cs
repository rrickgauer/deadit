using Deadit.WebGui.Controllers.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Deadit.WebGui.Controllers.Gui;

[Controller]
[Route("/auth")]
public class LoginController : Controller, IControllerName
{
    public static string ControllerRedirectName => IControllerName.RemoveControllerSuffix(nameof(LoginController));

    /// <summary>
    /// /auth/login
    /// </summary>
    /// <returns></returns>
    [HttpGet("login")]
    [ActionName(nameof(LoginPageAsync))]
    public async Task<IActionResult> LoginPageAsync([FromQuery] string? dest)
    {
        return Ok("login page");
    }
}
