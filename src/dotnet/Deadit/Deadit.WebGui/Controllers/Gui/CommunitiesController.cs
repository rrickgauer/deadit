using Deadit.Lib.Domain.Constants;
using Deadit.Lib.Service.Contracts;
using Deadit.Lib.Service.ViewModels;
using Deadit.WebGui.Controllers.Contracts;
using Deadit.WebGui.Filter;
using Microsoft.AspNetCore.Mvc;

namespace Deadit.WebGui.Controllers.Gui;

[Controller]
[Route("/communities")]
public class CommunitiesController(JoinedCommunitiesPageVMService joinedCommunitiesPageVMService) : GuiController, IControllerName
{
    // IControllerName
    public static string ControllerRedirectName => IControllerName.RemoveControllerSuffix(nameof(CommunitiesController));


    private readonly JoinedCommunitiesPageVMService _joinedCommunitiesPageVM = joinedCommunitiesPageVMService;

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
        var getViewModelResponse = await _joinedCommunitiesPageVM.GetViewModelAsync(new()
        {
            ClientId = ClientId!.Value,
        });

        if (!getViewModelResponse.Successful)
        {
            return BadRequest(getViewModelResponse);
        }

        return View(GuiPageViewFiles.JoinedCommunitiesPage, getViewModelResponse.Data);
    }



}
