using Deadit.Lib.Domain.Constants;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Filter;
using Deadit.Lib.Service.ViewModels;
using Deadit.WebGui.Controllers.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Deadit.WebGui.Controllers.Gui;

[Controller]
[Route("/c/{communityName}/posts/{postId:guid}")]
[ServiceFilter(typeof(CanViewCommunityFilter))]
public class PostController(PostPageVMService postPageVMService) : GuiController, IControllerName
{
    // IControllerName
    public static string ControllerRedirectName => IControllerName.RemoveControllerSuffix(nameof(PostController));

    private readonly PostPageVMService _postPageVMService = postPageVMService;

    /// <summary>
    /// GET: /c/:communityName/posts/:postId
    /// </summary>
    /// <param name="communityName"></param>
    /// <param name="postId"></param>
    /// <param name="sort"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    [HttpGet]
    [ActionName(nameof(GetPostPageAsync))]
    [ServiceFilter(typeof(PostExistsFilter))]
    public async Task<IActionResult> GetPostPageAsync([FromRoute] string communityName, [FromRoute] Guid postId, [FromQuery] SortOption? sort)
    {
        SortOption sortOption = sort ?? SortOption.New;

        PostType postType = RequestItems.Post?.PostType ?? throw new ArgumentNullException();

        var getViewModel = await _postPageVMService.GetViewModelAsync(new()
        {
            ClientId = ClientId,
            PostId = postId,
            PostType = postType,
            SortOption = sortOption,
        });

        if (!getViewModel.Successful)
        {
            return BadRequest(getViewModel);
        }

        return View(GuiPageViewFiles.PostPage, getViewModel.Data);
    }
}
