using Deadit.Lib.Domain.Dto;
using Deadit.Lib.Domain.Response;
using Deadit.Lib.Filter;
using Deadit.Lib.Service.Contracts;
using Deadit.WebGui.Controllers.Contracts;
using Deadit.WebGui.Filter;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Deadit.WebGui.Controllers.Api;

[ApiController]
[Route("api/communities/{communityName}/members")]
[ServiceFilter(typeof(CommunityNameExistsFilter))]
public class ApiCommunityMembershipController(ICommunityMemberService communityMemberService) : InternalApiController, IControllerName
{
    // IControllerName
    public static string ControllerRedirectName => IControllerName.RemoveControllerSuffix(nameof(ApiCommunityMembershipController));

    private readonly ICommunityMemberService _communityMemberService = communityMemberService;

    /// <summary>
    /// PUT: /api/communities/:communityName/members
    /// </summary>
    /// <param name="communityName"></param>
    /// <returns></returns>
    [HttpPut]
    [ServiceFilter(typeof(InternalApiAuthFilter))]
    [ActionName(nameof(JoinCommunityAsync))]
    public async Task<ActionResult<ApiResponse<GetJoinedCommunity>>> JoinCommunityAsync([FromRoute] string communityName)
    {        
        var newCommunityMembership = await _communityMemberService.JoinCommunityAsync(ClientId!.Value, communityName);

        if (!newCommunityMembership.Successful)
        {
            return BadRequest(newCommunityMembership);
        }

        string uri = $"{Request.GetEncodedUrl()}/{ClientId}";
        return Created(uri, newCommunityMembership);
    }

    [HttpDelete]
    [ServiceFilter(typeof(InternalApiAuthFilter))]
    [ActionName(nameof(LeaveCommunityAsync))]
    public async Task<IActionResult> LeaveCommunityAsync([FromRoute] string communityName)
    { 
        var serviceResponse = await _communityMemberService.LeaveCommunityAsync(ClientId!.Value, communityName);

        if (!serviceResponse.Successful)
        {
            return BadRequest(serviceResponse);
        }
        
        return NoContent();
    }

}
