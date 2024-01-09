using Deadit.Lib.Domain.Constants;
using Deadit.Lib.Filter;
using Deadit.Lib.Service.Contracts;
using Deadit.WebGui.Controllers.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Deadit.WebGui.Controllers.Gui;

[Controller]
[Route("/c")]
public class CommunityController : Controller, IControllerName
{
    // IControllerName
    public static string ControllerRedirectName => IControllerName.RemoveControllerSuffix(nameof(CommunityController));
    
    private readonly ICommunityService _communityService;

    public CommunityController(ICommunityService communityService)
    {
        _communityService = communityService;
    }

    /// <summary>
    /// /c/:communityName
    /// </summary>
    /// <returns></returns>
    [HttpGet("{communityName}")]
    [ActionName(nameof(GetCommunityPage))]
    [ServiceFilter(typeof(CommunityNameExistsFilter))]
    public async Task<IActionResult> GetCommunityPage([FromRoute] string communityName)
    {
        var serviceResponse = await _communityService.GetCommunityPageViewModelAsync(communityName);
        return View(GuiPageViewFiles.CommunityPage, serviceResponse.Data);
    }
}
