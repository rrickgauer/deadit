using Deadit.Lib.Auth;
using Deadit.Lib.Domain.Constants;
using Deadit.Lib.Filter;
using Deadit.Lib.Service.Contracts;
using Deadit.WebGui.Controllers.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Contracts;

namespace Deadit.WebGui.Controllers.Gui;

[Controller]
[Route("/c/{communityName}")]
[ServiceFilter(typeof(CommunityNameExistsFilter))]
public class CommunityController : Controller, IControllerName
{
    // IControllerName
    public static string ControllerRedirectName => IControllerName.RemoveControllerSuffix(nameof(CommunityController));

    public SessionManager SessionManager => new(Request.HttpContext.Session);
    public uint? ClientId => SessionManager.ClientId;


    
    private readonly ICommunityService _communityService;

    public CommunityController(ICommunityService communityService)
    {
        _communityService = communityService;
    }

    /// <summary>
    /// /c/:communityName
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ActionName(nameof(GetCommunityPage))]
    public async Task<IActionResult> GetCommunityPage([FromRoute] string communityName)
    {
        var serviceResponse = await _communityService.GetCommunityPageViewModelAsync(communityName, ClientId);
        return View(GuiPageViewFiles.CommunityPage, serviceResponse.Data);
    }


    /// <summary>
    /// /c/:communityName/submit
    /// </summary>
    /// <param name="communityName"></param>
    /// <returns></returns>
    [HttpGet("submit")]
    [ActionName(nameof(GetNewPostCommunityPage))]
    public async Task<IActionResult> GetNewPostCommunityPage([FromRoute] string communityName)
    {
        return Ok("create new post");
    }


}
