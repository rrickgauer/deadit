using Deadit.Lib.Domain.Constants;
using Deadit.Lib.Service.Contracts;
using Deadit.Lib.Service.ViewModels;
using Deadit.WebGui.Controllers.Contracts;
using Deadit.WebGui.Filter;
using Microsoft.AspNetCore.Mvc;

namespace Deadit.WebGui.Controllers.Gui;

[Controller]
[Route("/communities")]
public class CommunitiesController(JoinedCommunitiesPageVMService joinedCommunitiesPageVMService, CreatedCommunitiesPageVMService createdCommunitiesPageVMService) : GuiController, IControllerName
{
    // IControllerName
    public static string ControllerRedirectName => IControllerName.RemoveControllerSuffix(nameof(CommunitiesController));

    private readonly JoinedCommunitiesPageVMService _joinedCommunitiesPageVM = joinedCommunitiesPageVMService;
    private readonly CreatedCommunitiesPageVMService _createdCommunitiesPageVMService = createdCommunitiesPageVMService;

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


    [HttpGet("created")]
    [ActionName(nameof(CreatedCommuntiesPageAsync))]
    [ServiceFilter(typeof(LoginFirstRedirectFilter))]
    public async Task<IActionResult> CreatedCommuntiesPageAsync()
    {
        var getViewModel = await _createdCommunitiesPageVMService.GetViewModelAsync(new()
        {
            UserId = ClientId!.Value,
        });

        if (!getViewModel.Successful)
        {
            return BadRequest(getViewModel);
        }

        return View(GuiPageViewFiles.CreatedCommunitiesPage, getViewModel.Data);
    }



}
