using Deadit.Lib.Auth;
using Deadit.Lib.Domain.Errors;
using Deadit.Lib.Domain.Forms;
using Deadit.Lib.Domain.TableView;
using Deadit.Lib.Service.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Deadit.WebGui.Controllers.Api;

[Route("api/auth")]
[ApiController]
//[ServiceFilter(typeof(InternalApiAuthFilter))]
public class ApiAuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IResponseService _responseService;

    public ApiAuthController(IAuthService authService, IResponseService responseService)
    {
        _authService = authService;
        _responseService = responseService;
    }

    [HttpPost("signup")]
    public async Task<IActionResult> PostSignupAsync([FromForm] SignupRequestForm signupForm)
    {
        ServiceDataResponse<ViewUser> newUserResponse = await _authService.SignupUserAsync(signupForm);
        var apiResponse = await _responseService.ToApiResponseAsync(newUserResponse);

        if (!newUserResponse.Successful)
        {
            return BadRequest(apiResponse);
        }

        return Created($"/users/{newUserResponse.Data?.Id}", apiResponse);
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
