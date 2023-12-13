using Deadit.Lib.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Deadit.WebGui.Controllers.Api;

[Route("api/auth")]
[ApiController]
[ServiceFilter(typeof(InternalApiAuthFilter))]
public class ApiAuthController : ControllerBase
{

    [HttpPost("signup")]
    public async Task<IActionResult> PostSignupAsync()
    {


        return Ok();
    }

}
