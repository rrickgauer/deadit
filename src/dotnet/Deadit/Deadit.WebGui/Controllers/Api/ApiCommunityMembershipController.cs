using Deadit.Lib.Filter;
using Deadit.Lib.Service.Contracts;
using Deadit.WebGui.Controllers.Contracts;
using Deadit.WebGui.Filter;
using Microsoft.AspNetCore.Mvc;

namespace Deadit.WebGui.Controllers.Api;

[ApiController]
[Route("api/communities/{communityName}/members")]
[ServiceFilter(typeof(CommunityNameExistsFilter))]
public class ApiCommunityMembershipController : InternalApiControllerBase, IControllerName
{
    // IControllerName
    public static string ControllerRedirectName => IControllerName.RemoveControllerSuffix(nameof(ApiCommunityMembershipController));

    private readonly ICommunityMemberService _communityMemberService;
    private readonly IResponseService _responseService;
    

    public ApiCommunityMembershipController(ICommunityMemberService communityMemberService, IResponseService responseService)
    {
        _communityMemberService = communityMemberService;
        _responseService = responseService;
    }

    /// <summary>
    /// PUT: /api/communities/:communityName/members
    /// </summary>
    /// <param name="communityName"></param>
    /// <returns></returns>
    [HttpPut]
    [ServiceFilter(typeof(InternalApiAuthFilter))]
    [ActionName(nameof(JoinCommunityAsync))]
    public async Task<IActionResult> JoinCommunityAsync([FromRoute] string communityName)
    {
        int x = 10;

        return Ok();
    }

    [HttpDelete]
    [ServiceFilter(typeof(InternalApiAuthFilter))]
    [ActionName(nameof(LeaveCommunityAsync))]
    public async Task<IActionResult> LeaveCommunityAsync([FromRoute] string communityName)
    { 
        var serviceResponse = await _communityMemberService.LeaveCommunityAsync(ClientId.Value, communityName);

        if (!serviceResponse.Successful)
        {
            var apiResponse = await _responseService.ToApiResponseAsync(serviceResponse);
            return BadRequest(apiResponse);
        }
        
        return NoContent();
    }

}
