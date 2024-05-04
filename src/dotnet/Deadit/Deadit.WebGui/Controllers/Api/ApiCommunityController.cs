using Deadit.Lib.Domain.Forms;
using Deadit.Lib.Service.Contracts;
using Deadit.WebGui.Controllers.Contracts;
using Deadit.WebGui.Filter;
using Microsoft.AspNetCore.Mvc;

namespace Deadit.WebGui.Controllers.Api;

[ApiController]
[Route("api/communities")]
public class ApiCommunityController(ICommunityService communityService) : InternalApiController, IControllerName
{
    public static string ControllerRedirectName => IControllerName.RemoveControllerSuffix(nameof(ApiCommunityController));

    private readonly ICommunityService _communityService = communityService;

    /// <summary>
    /// POST: /communities
    /// </summary>
    /// <param name="newCommunityForm"></param>
    /// <returns></returns>
    [HttpPost]
    [ServiceFilter(typeof(InternalApiAuthFilter))]
    [ActionName(nameof(PostCommunityAsync))]
    public async Task<IActionResult> PostCommunityAsync([FromForm] CreateCommunityRequestForm newCommunityForm)
    {
        var community = await _communityService.CreateCommunityAsync(newCommunityForm, ClientId!.Value);
        
        if (community.Successful)
        {
            return Created($"/communities/{community.Data?.CommunityId}", community);    
        }

        return BadRequest(community);
    }

}
