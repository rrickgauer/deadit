using Deadit.Lib.Domain.Constants;
using Deadit.WebGui.Controllers.Contracts;
using Deadit.WebGui.Filter;
using Microsoft.AspNetCore.Mvc;

namespace Deadit.WebGui.Controllers.Gui;

[Controller]
[Route("/communities")]
public class CommunitiesController : Controller, IControllerName
{
    public static string ControllerRedirectName => IControllerName.RemoveControllerSuffix(nameof(CommunitiesController));

    /// <summary>
    /// GET: /communities
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> CommunitiesPageAsync()
    {
        return View(GuiPageViewFiles.CommunitiesPage);
    }

    /// <summary>
    /// GET: /communities/create
    /// </summary>
    /// <returns></returns>
    [HttpGet("create")]
    [ServiceFilter(typeof(LoginFirstRedirectFilter))]
    public async Task<IActionResult> CreateCommunityPageAsync()
    {
        return View(GuiPageViewFiles.CreateCommunitiesPage);
    }

    [HttpGet("{communityName}")]
    public async Task<IActionResult> ViewCommunityPageAsync([FromRoute] string communityName)
    {
        var output = new
        {
            CommunityName = communityName,
            Bitch = true,
        };


        return RedirectToAction(nameof(CommunityController.GetCommunityPage), CommunityController.ControllerRedirectName, new
        {
            communityName = communityName,
        });

    }

}
