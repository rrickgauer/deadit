using Microsoft.AspNetCore.Mvc;

namespace Deadit.WebGui.Controllers.Gui;

[Controller]
[Route("/communities")]
public class CommunitiesController : Controller
{

    /// <summary>
    /// GET: /communities
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> CommunitiesPageAsync()
    {
        return View("CommunitiesPage");
    }

    /// <summary>
    /// GET: /communities/create
    /// </summary>
    /// <returns></returns>
    [HttpGet("create")]
    public async Task<IActionResult> CreateCommunityPageAsync()
    {
        return View("Views/CreateCommunity/CreateCommunityPage.cshtml");
    }

}
