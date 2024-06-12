using Deadit.Lib.Domain.Constants;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Forms;
using Deadit.Lib.Domain.Other;
using Deadit.Lib.Domain.Paging;
using Deadit.Lib.Filter;
using Deadit.Lib.Service.ViewModels;
using Deadit.WebGui.Controllers.Contracts;
using Deadit.WebGui.Filter;
using Microsoft.AspNetCore.Mvc;

namespace Deadit.WebGui.Controllers.Gui;


[Controller]
[Route("/c/{communityName}/settings")]
[ServiceFilter(typeof(LoginFirstRedirectFilter))]
[ServiceFilter(typeof(ModifyCommunityFilter))]
public class CommunitySettingsController : GuiController, IControllerName
{
    // IControllerName
    public static string ControllerRedirectName => IControllerName.RemoveControllerSuffix(nameof(CommunitySettingsController));

    private readonly GeneralCommunitySettingsVMService _generalVMService;
    private readonly MembersCommunitySettingsVMService _memberVMService;
    private readonly ContentCommunitySettingsVMService _contentVMService;

    public CommunitySettingsController(GeneralCommunitySettingsVMService generalVMService, MembersCommunitySettingsVMService memberVMService, ContentCommunitySettingsVMService contentVMService)
    {
        _generalVMService = generalVMService;
        _memberVMService = memberVMService;
        _contentVMService = contentVMService;
    }

    [HttpGet]
    [HttpGet("general")]
    [ActionName(nameof(GetGeneralSettingsPageAsync))]
    public async Task<IActionResult> GetGeneralSettingsPageAsync([FromRoute] string communityName)
    {
        var getViewModel = await _generalVMService.GetViewModelAsync(new()
        {
            CommunityName = communityName,
        });

        if (!getViewModel.Successful)
        {
            return BadRequest(getViewModel);
        }

        return View(GuiPageViewFiles.CommunitySettingsGeneralPage, getViewModel.Data);
    }

    [HttpGet("members")]
    [ActionName(nameof(GetMembersSettingsPageAsync))]
    public async Task<IActionResult> GetMembersSettingsPageAsync([FromRoute] string communityName, [FromQuery] GetMembersSettingsPageQueryParms? queryParms)
    {
        var getViewModel = await _memberVMService.GetViewModelAsync(new()
        {
            CommunityName = communityName,
            CommunityMembersSorting = new(queryParms?.Sort, queryParms?.SortDirection),
            Pagination = new(queryParms?.Page),
        });

        if (!getViewModel.Successful)
        {
            return BadRequest(getViewModel);
        }

        return View(GuiPageViewFiles.CommunitySettingsMembersPage, getViewModel.Data);
    }

    [HttpGet("content")]
    [ActionName(nameof(GetContentSettingsPageAsync))]
    public async Task<IActionResult> GetContentSettingsPageAsync([FromRoute] string communityName)
    {
        var getViewModel = await _contentVMService.GetViewModelAsync(new()
        {
            CommunityName = communityName,
        });

        if (!getViewModel.Successful)
        {
            return BadRequest(getViewModel);
        }

        return View(GuiPageViewFiles.CommunitySettingsContentPage, getViewModel.Data);
    }
}
