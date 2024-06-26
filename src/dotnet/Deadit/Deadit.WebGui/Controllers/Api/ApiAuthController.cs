﻿using Deadit.Lib.Domain.Forms;
using Deadit.Lib.Domain.Response;
using Deadit.Lib.Domain.TableView;
using Deadit.Lib.Service.Contracts;
using Deadit.WebGui.Controllers.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Deadit.WebGui.Controllers.Api;


[Route("api/auth")]
[ApiController]
public class ApiAuthController(IAuthService authService) : InternalApiController, IControllerName
{
    public static string ControllerRedirectName => IControllerName.RemoveControllerSuffix(nameof(ApiAuthController));

    private readonly IAuthService _authService = authService;

    /// <summary>
    /// POST: /api/auth/signup
    /// </summary>
    /// <param name="signupForm"></param>
    /// <returns></returns>
    [HttpPost("signup")]
    public async Task<ActionResult<ApiResponse<ViewUser>>> PostSignupAsync([FromForm] SignupRequestForm signupForm)
    {
        ServiceDataResponse<ViewUser> newUserResponse = await _authService.SignupUserAsync(signupForm);

        if (!newUserResponse.Successful)
        {
            return BadRequest(newUserResponse);
        }

        return Created($"/users/{newUserResponse.Data?.UserId}", newUserResponse);
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
        var apiResponse = new EmptyApiResponse();

        if (!result.HasData)
        {
            return NotFound(apiResponse);
        }

        return Ok(apiResponse);
    }

}
