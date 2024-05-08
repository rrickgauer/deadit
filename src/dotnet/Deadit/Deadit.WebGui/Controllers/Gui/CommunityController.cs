﻿using Deadit.Lib.Domain.Constants;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Filter;
using Deadit.Lib.Service.Contracts;
using Deadit.WebGui.Controllers.Contracts;
using Deadit.WebGui.Filter;
using Microsoft.AspNetCore.Mvc;

namespace Deadit.WebGui.Controllers.Gui;

[Controller]
[Route("/c/{communityName}")]
[ServiceFilter(typeof(CommunityNameExistsFilter))]
public class CommunityController(IViewModelService viewModelService, IPostService postService, ICommentService commentService) : GuiController, IControllerName
{
    // IControllerName
    public static string ControllerRedirectName => IControllerName.RemoveControllerSuffix(nameof(CommunityController));

    private readonly IViewModelService _viewModelService = viewModelService;
    private readonly IPostService _postService = postService;
    private readonly ICommentService _commentService = commentService;


    /// <summary>
    /// /c/:communityName
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ActionName(nameof(GetCommunityPage))]
    public async Task<IActionResult> GetCommunityPage([FromRoute] string communityName)
    {
        var serviceResponse = await _viewModelService.GetCommunityPageViewModelAsync(communityName, ClientId);

        if (!serviceResponse.Successful)
        {
            return BadRequest(serviceResponse);
        }

        return View(GuiPageViewFiles.CommunityPage, serviceResponse.Data);
    }


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


    [HttpGet("posts/{postId}")]
    [ActionName(nameof(GetPostPageAsync))]
    [ServiceFilter(typeof(GetPostFilter))]
    public async Task<IActionResult> GetPostPageAsync([FromRoute] string communityName, [FromRoute] Guid postId)
    {
        PostType postType = RequestItems.Post?.PostType ?? throw new ArgumentNullException();

        var getViewModel = await _viewModelService.GetPostPageViewModelAsync(postId, postType, ClientId);

        if (!getViewModel.Successful)
        {
            return BadRequest(getViewModel);
        }

        return View(GuiPageViewFiles.PostPage, getViewModel.Data);
    }


}
