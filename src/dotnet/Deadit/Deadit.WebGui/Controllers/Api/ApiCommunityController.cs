using Deadit.Lib.Domain.Forms;
using Deadit.Lib.Filters;
using Deadit.Lib.Service.Contracts;
using Deadit.WebGui.Controllers.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Deadit.WebGui.Controllers.Api;

[ApiController]
[Route("api/communities")]
public class ApiCommunityController : InternalApiControllerBase, IControllerName
{
    public static string ControllerRedirectName => IControllerName.RemoveControllerSuffix(nameof(ApiCommunityController));

    private readonly ICommunityService _communityService;
    private readonly IResponseService _responseService;
    

    public ApiCommunityController(ICommunityService communityService, IResponseService responseService)
    {
        _communityService = communityService;
        _responseService = responseService;
    }

    

    /// <summary>
    /// POST: /communities
    /// </summary>
    /// <param name="newCommunityForm"></param>
    /// <returns></returns>
    [HttpPost]
    [ServiceFilter(typeof(InternalApiAuthFilter))]
    public async Task<IActionResult> PostCommunityAsync([FromForm] CreateCommunityRequestForm newCommunityForm)
    {
        var community = await _communityService.CreateCommunityAsync(newCommunityForm, ClientId);
        var response = await _responseService.ToApiResponseAsync(community);
        
        if (community.Successful)
        {
            return Created($"/communities/{community.Data?.Id}", response);    
        }

        return BadRequest(response);
    }



}
