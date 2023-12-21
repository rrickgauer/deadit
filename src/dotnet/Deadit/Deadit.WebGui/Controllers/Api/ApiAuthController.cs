using Deadit.Lib.Auth;
using Deadit.Lib.Domain.Forms;
using Deadit.Lib.Service.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Deadit.WebGui.Controllers.Api;

[Route("api/auth")]
[ApiController]
//[ServiceFilter(typeof(InternalApiAuthFilter))]
public class ApiAuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public ApiAuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("signup")]
    public async Task<IActionResult> PostSignupAsync([FromForm] SignupRequestForm signupForm)
    {
        var newUser = await _authService.SignupUserAsync(signupForm);

        if (newUser == null)
        {
            return BadRequest("Could not create your account");
        }

        return Ok(newUser);
    }


    [HttpPost("login")]
    public async Task<IActionResult> PostLoginAsync([FromForm] LoginRequestForm loginForm)
    {
        var user = await _authService.LoginUserAsync(loginForm, HttpContext.Session);

        if (user == null)
        {
            return NotFound();
        }

        return Ok();
    }

}
