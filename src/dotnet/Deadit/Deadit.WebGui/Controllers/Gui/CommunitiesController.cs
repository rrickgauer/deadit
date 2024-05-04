using Deadit.Lib.Domain.Constants;
using Deadit.Lib.Service.Contracts;
using Deadit.WebGui.Controllers.Contracts;
using Deadit.WebGui.Filter;
using Microsoft.AspNetCore.Mvc;

namespace Deadit.WebGui.Controllers.Gui;

[Controller]
[Route("/communities")]
public class CommunitiesController(IViewModelService viewModelService) : GuiController, IControllerName
{
    // IControllerName
    public static string ControllerRedirectName => IControllerName.RemoveControllerSuffix(nameof(CommunitiesController));

    private readonly IViewModelService _viewModelService = viewModelService;

    /// <summary>
    /// GET: /communities
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ActionName(nameof(CommunitiesPageAsync))]
    public IActionResult CommunitiesPageAsync()
    {
        return View(GuiPageViewFiles.CommunitiesPage);
    }

    /// <summary>
    /// GET: /communities/create
    /// </summary>
    /// <returns></returns>
    [HttpGet("create")]
    [ServiceFilter(typeof(LoginFirstRedirectFilter))]
    [ActionName(nameof(CreateCommunityPageAsync))]
    public IActionResult CreateCommunityPageAsync()
    {
        return View(GuiPageViewFiles.CreateCommunitiesPage);
    }


    [HttpGet("joined")]
    [ServiceFilter(typeof(LoginFirstRedirectFilter))]
    [ActionName(nameof(JoinedCommunitiesPage))]
    public async Task<IActionResult> JoinedCommunitiesPage()
    {
        var getViewModelResponse = await _viewModelService.GetJoinedCommunitiesPageViewModelAsync(ClientId!.Value);

        if (!getViewModelResponse.Successful)
        {
            return BadRequest(getViewModelResponse);
        }

        return View(GuiPageViewFiles.JoinedCommunitiesPage, getViewModelResponse.Data);
    }

    

}
