using Deadit.Lib.Domain.Forms;
using Deadit.Lib.Domain.Model;
using Deadit.Lib.Filter;
using Deadit.Lib.Service.Contracts;
using Deadit.WebGui.Controllers.Contracts;
using Deadit.WebGui.Filter;
using Microsoft.AspNetCore.Mvc;

namespace Deadit.WebGui.Controllers.Api;


[ApiController]
[Route("api/communities/{communityName}/posts/{postId:guid}/comments")]
[ServiceFilter(typeof(GetPostFilter))]
public class ApiCommentsController(IViewModelService viewModelService, ICommentService commentService) : InternalApiController, IControllerName
{
    // IControllerName
    public static string ControllerRedirectName => IControllerName.RemoveControllerSuffix(nameof(ApiCommentsController));

    private readonly IViewModelService _viewModelService = viewModelService;
    private readonly ICommentService _commentService = commentService;


    /// <summary>
    /// GET: /api/communities/:communityName/posts/:postId/comments
    /// </summary>
    /// <param name="communityName"></param>
    /// <param name="postId"></param>
    /// <returns></returns>
    [HttpGet]
    [ActionName(nameof(GetCommentsAsync))]
    public async Task<IActionResult> GetCommentsAsync([FromRoute] string communityName, [FromRoute] Guid postId)
    {
        var getComments = await _viewModelService.GetCommentsApiResponseAsync(postId, ClientId);

        if (!getComments.Successful)
        {
            return BadRequest(getComments);
        }

        return Ok(getComments);
    }

    /// <summary>
    /// PUT: /api/communities/:communityName/posts/:postId/comments/:commentId
    /// </summary>
    /// <param name="communityName"></param>
    /// <param name="postId"></param>
    /// <param name="commentId"></param>
    /// <returns></returns>
    [HttpPut("{commentId:guid}")]
    [ActionName(nameof(PutCommentAsync))]
    [ServiceFilter(typeof(InternalApiAuthFilter))]
    [ServiceFilter(typeof(CommunityMemberFilter))]
    public async Task<IActionResult> PutCommentAsync([FromRoute] string communityName, [FromRoute] Guid postId, [FromRoute] Guid commentId, [FromBody] CommentForm commentForm)
    {
        Comment comment = new()
        {
            PostId = postId,
            AuthorId = ClientId,
            Content = commentForm.Content,
            CreatedOn = DateTime.UtcNow,
            Id = commentId,
            ParentId = commentForm.ParentId,
        };

        var saveComment = await _commentService.SaveCommentAsync(comment);

        if (!saveComment.Successful)
        {
            return BadRequest(saveComment);
        }

        var getComment = await _viewModelService.GetCommentApiResponseAsync(commentId, ClientId);

        return ReturnStandardDataResponse(getComment);
    }
}
