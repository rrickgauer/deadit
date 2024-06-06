using Deadit.Lib.Domain.Forms;
using Deadit.Lib.Domain.Model;
using Deadit.Lib.Filter;
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
    public async Task<IActionResult> PostCommunityAsync([FromBody] CreateCommunityRequestForm newCommunityForm)
    {
        var community = await _communityService.CreateCommunityAsync(newCommunityForm, ClientId!.Value);
        
        if (community.Successful)
        {
            return Created($"/communities/{community.Data?.CommunityId}", community);    
        }

        return BadRequest(community);
    }


    [HttpPut("{communityName}")]
    [ServiceFilter(typeof(InternalApiAuthFilter))]
    [ServiceFilter(typeof(ModifyCommunityFilter))]
    [ActionName(nameof(PutCommunityAsync))]
    public async Task<IActionResult> PutCommunityAsync([FromRoute] string communityName, [FromBody] UpdateCommunityRequestForm data)
    {
        Community community = new()
        {
            Id = RequestItems.CommunityId,
            Name = communityName,
        };

        data.SetCommunityData(community);

        var updateResult = await _communityService.SaveCommunityAsync(community);

        if (!updateResult.Successful)
        {
            return BadRequest(updateResult);
        }

        return Ok(updateResult);
    }

}
