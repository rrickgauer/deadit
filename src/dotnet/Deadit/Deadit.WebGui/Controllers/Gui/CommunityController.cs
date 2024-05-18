using Deadit.Lib.Domain.Constants;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Other;
using Deadit.Lib.Domain.Paging;
using Deadit.Lib.Filter;
using Deadit.Lib.Service.Contracts;
using Deadit.Lib.Service.ViewModels;
using Deadit.WebGui.Controllers.Contracts;
using Deadit.WebGui.Filter;
using Microsoft.AspNetCore.Mvc;

namespace Deadit.WebGui.Controllers.Gui;

[Controller]
[Route("/c/{communityName}")]
[ServiceFilter(typeof(CommunityNameExistsFilter))]
public class CommunityController(CommunityPageVMService communityPage, IPostService postService) : GuiController, IControllerName
{
    // IControllerName
    public static string ControllerRedirectName => IControllerName.RemoveControllerSuffix(nameof(CommunityController));

    private readonly CommunityPageVMService _communityPage = communityPage;

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
        return View(GuiPageViewFiles.CreatePostPage);
    }


    /// <summary>
    /// /c/:communityName
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ActionName(nameof(GetCommunityPageAsync))]
    public async Task<IActionResult> GetCommunityPageAsync([FromRoute] string communityName, [FromQuery] uint? page)
    {
        // og shit below
        PostSorting postSorting = new(CommunityPagePostSort.New);

        return await ReturnCommunityPageAsync(communityName, postSorting, page);
    }

    /// <summary>
    /// GET: /c/:communityName/top?sort=day,week,month,year,all
    /// </summary>
    /// <param name="communityName"></param>
    /// <param name="sort"></param>
    /// <returns></returns>
    [HttpGet("top")]
    [ActionName(nameof(GetCommunityPageTop))]
    public async Task<IActionResult> GetCommunityPageTop([FromRoute] string communityName, [FromQuery] TopPostSort? sort, [FromQuery] uint? page)
    {
        TopPostSort topSort = sort ?? TopPostSort.Month;

        PostSorting postSorting = new(CommunityPagePostSort.Top, topSort);

        return await ReturnCommunityPageAsync(communityName, postSorting, page);
    }


    private async Task<IActionResult> ReturnCommunityPageAsync(string communityName, PostSorting postSorting, uint? page)
    {
        PaginationPosts pagination = new(page);

        var serviceResponse = await _communityPage.GetViewModelAsync(new()
        {
            ClientId = ClientId,
            CommunityName = communityName,
            PostSorting = postSorting,
            Pagination = pagination,
        });

        if (!serviceResponse.Successful)
        {
            return BadRequest(serviceResponse);
        }

        return View(GuiPageViewFiles.CommunityPage, serviceResponse.Data);
    }









}
