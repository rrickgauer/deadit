using Deadit.Lib.Domain.Constants;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Forms;
using Deadit.Lib.Domain.Other;
using Deadit.Lib.Domain.Paging;
using Deadit.Lib.Filter;
using Deadit.Lib.Service.Contracts;
using Deadit.Lib.Service.ViewModels;
using Deadit.WebGui.Controllers.Contracts;
using Deadit.WebGui.Filter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Deadit.WebGui.Controllers.Gui;

[Controller]
[Route("/c/{communityName}")]
[ServiceFilter(typeof(CanViewCommunityFilter))]
public class CommunityController(CommunityPageVMService communityPageVMService, IPostService postService, NewPostPageVMService newPostVMService) : GuiController, IControllerName
{
    // IControllerName
    public static string ControllerRedirectName => IControllerName.RemoveControllerSuffix(nameof(CommunityController));

    private readonly CommunityPageVMService _communityPageVMService = communityPageVMService;
    private readonly NewPostPageVMService _newPostVMService = newPostVMService;

    private readonly IPostService _postService = postService;

    /// <summary>
    /// /c/:communityName/submit
    /// </summary>
    /// <param name="communityName"></param>
    /// <returns></returns>
    [HttpGet("submit")]
    [ServiceFilter(typeof(LoginFirstRedirectFilter))]
    [ActionName(nameof(GetNewPostCommunityPage))]
    public async Task<IActionResult> GetNewPostCommunityPage([FromRoute] string communityName)
    {
        var getVM = await _newPostVMService.GetViewModelAsync(new()
        {
            CommunityName = communityName,
        });

        if (!getVM.Successful)
        {
            return BadRequest(getVM);
        }

        return View(GuiPageViewFiles.CreatePostPage, getVM.Data);
    }


    /// <summary>
    /// /c/:communityName
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ActionName(nameof(GetCommunityPageAsync))]
    public async Task<IActionResult> GetCommunityPageAsync([FromRoute] string communityName, [FromQuery] GetCommunityPageQueryParms queryParms)
    {
        return await ReturnCommunityPageAsync(communityName, queryParms);
    }

    /// <summary>
    /// GET: /c/:communityName/top?sort=day,week,month,year,all
    /// </summary>
    /// <param name="communityName"></param>
    /// <param name="sort"></param>
    /// <returns></returns>
    [HttpGet("top")]
    [ActionName(nameof(GetCommunityPageTop))]
    public async Task<IActionResult> GetCommunityPageTop([FromRoute] string communityName, [FromQuery] GetCommunityPageQueryParmsTop queryParms)
    {
        return await ReturnCommunityPageAsync(communityName, queryParms);
    }


    private async Task<IActionResult> ReturnCommunityPageAsync(string communityName, BaseGetCommunityPageQueryParms queryParms)
    {
        PaginationPosts pagination = new(queryParms.Page);

        var serviceResponse = await _communityPageVMService.GetViewModelAsync(new()
        {
            ClientId = ClientId,
            CommunityName = communityName,
            PostSorting = queryParms.GetPostSorting(),
            Pagination = pagination,
            FilterByFlairId = queryParms.Flair,
        });

        if (!serviceResponse.Successful)
        {
            return BadRequest(serviceResponse);
        }

        return View(GuiPageViewFiles.CommunityPage, serviceResponse.Data);
    }
}
