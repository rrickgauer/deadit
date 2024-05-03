using Deadit.Lib.Domain.Constants;
using Deadit.Lib.Domain.TableView;
using Deadit.Lib.Service.Contracts;
using Deadit.WebGui.Controllers.Contracts;
using Deadit.WebGui.Filter;
using Microsoft.AspNetCore.Mvc;

namespace Deadit.WebGui.Controllers.Gui;

[Controller]
[Route("/communities")]
public class CommunitiesController(ICommunityService communityService, ICommunityMemberService memberService) : GuiController, IControllerName
{
    // IControllerName
    public static string ControllerRedirectName => IControllerName.RemoveControllerSuffix(nameof(CommunitiesController));

    private readonly ICommunityService _communityService = communityService;
    private readonly ICommunityMemberService _memberService = memberService;


    /// <summary>
    /// GET: /communities
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ActionName(nameof(CommunitiesPageAsync))]
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
    [ActionName(nameof(CreateCommunityPageAsync))]
    public async Task<IActionResult> CreateCommunityPageAsync()
    {
        return View(GuiPageViewFiles.CreateCommunitiesPage);
    }


    [HttpGet("joined")]
    [ServiceFilter(typeof(LoginFirstRedirectFilter))]
    [ActionName(nameof(JoinedCommunitiesPage))]
    public async Task<IActionResult> JoinedCommunitiesPage()
    {
        var result = await _memberService.GetJoinedCommunitiesAsync(ClientId!.Value);

        return Ok(result);

        //var payload = result?.Data?.Select(m => (ViewCommunity)m);
        //return Ok(payload);
    }

    

}
