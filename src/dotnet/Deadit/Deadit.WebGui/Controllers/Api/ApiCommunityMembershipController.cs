using Deadit.Lib.Filter;
using Deadit.WebGui.Controllers.Contracts;
using Deadit.WebGui.Filter;
using Microsoft.AspNetCore.Mvc;

namespace Deadit.WebGui.Controllers.Api;

[ApiController]
[Route("api/communities/{communityName}/members")]
[ServiceFilter(typeof(CommunityNameExistsFilter))]
public class ApiCommunityMembershipController : ControllerBase, IControllerName
{
    // IControllerName
    public static string ControllerRedirectName => IControllerName.RemoveControllerSuffix(nameof(ApiCommunityController));

    [HttpPut("members")]
    [ServiceFilter(typeof(InternalApiAuthFilter))]
    [ActionName(nameof(JoinCommunityAsync))]
    public async Task<IActionResult> JoinCommunityAsync([FromRoute] string communityName)
    {
        return Ok();
    }

}
