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

    /// <summary>
    /// POST: /api/auth/signup
    /// </summary>
    /// <param name="signupForm"></param>
    /// <returns></returns>
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

    /// <summary>
    /// POST: /api/auth/login
    /// </summary>
    /// <param name="loginForm"></param>
    /// <returns></returns>
    [HttpPost("login")]
    public async Task<IActionResult> PostLoginAsync([FromForm] LoginRequestForm loginForm)
    {
        var result = await _authService.LoginUserAsync(loginForm, HttpContext.Session);
        var apiResponse = _responseService.GetEmptyApiResponse();

        if (!result.HasData)
        {
            return NotFound(apiResponse);
        }

        return Ok(apiResponse);
    }

}
