using Deadit.Lib.Service.Contracts;
using Deadit.WebGui.Controllers.Api;
using Deadit.WebGui.Controllers.Contracts;
using Microsoft.AspNetCore.Http.Extensions;
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
    public async Task<IActionResult> CreateCommunityPageAsync()
    {
        if (!IsClientLoggedIn)
        {
            return RedirectToAction(nameof(LoginController.LoginPageAsync), LoginController.ControllerRedirectName, new
            {
                destination = Request.GetEncodedUrl(),
            });
        }

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
