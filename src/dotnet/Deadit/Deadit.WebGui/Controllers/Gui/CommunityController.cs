using Deadit.Lib.Domain.Constants;
using Deadit.Lib.Filter;
using Deadit.WebGui.Controllers.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Deadit.WebGui.Controllers.Gui;

[Controller]
[Route("/c")]
public class CommunityController : Controller, IControllerName
{
    public static string ControllerRedirectName => IControllerName.RemoveControllerSuffix(nameof(CommunityController));

    /// <summary>
    /// /c/:communityName
    /// </summary>
    /// <returns></returns>
    [HttpGet("{communityName}")]
    [ActionName(nameof(GetCommunityPage))]
    [ServiceFilter(typeof(CommunityNameExistsFilter))]
    public async Task<IActionResult> GetCommunityPage([FromRoute] string communityName)
    {
        return View(GuiPageViewFiles.CommunityPage);
    }
}
