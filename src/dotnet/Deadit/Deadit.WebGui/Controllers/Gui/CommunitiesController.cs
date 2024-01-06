using Deadit.Lib.Service.Contracts;
using Deadit.WebGui.Controllers.Contracts;
using Deadit.WebGui.Filter;
using Microsoft.AspNetCore.Mvc;

namespace Deadit.WebGui.Controllers.Gui;

[Controller]
[Route("/communities")]
public class CommunitiesController : Controller, IControllerName
{
    public static string ControllerRedirectName => IControllerName.RemoveControllerSuffix(nameof(CommunitiesController));

    private readonly IAuthService _authService;

    private bool IsClientLoggedIn => _authService.IsClientLoggedIn();

    public CommunitiesController(IAuthService authService)
    {
        _authService = authService;
    }


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
    [ServiceFilter(typeof(LoginFirstRedirectFilter))]
    public async Task<IActionResult> CreateCommunityPageAsync()
    {
        return View("Views/CreateCommunity/CreateCommunityPage.cshtml");
    }

    [HttpGet("{communityName}")]
    public async Task<IActionResult> ViewCommunityPageAsync([FromRoute] string communityName)
    {
        var output = new
        {
            CommunityName = communityName,
            Bitch = true,
        };

        return Ok(output);
    }

}
