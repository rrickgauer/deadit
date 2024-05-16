using Deadit.Lib.Domain.Dto;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Forms;
using Deadit.Lib.Domain.Model;
using Deadit.Lib.Domain.Response;
using Deadit.Lib.Filter;
using Deadit.Lib.Service.Contracts;
using Deadit.WebGui.Controllers.Contracts;
using Deadit.WebGui.Filter;
using Microsoft.AspNetCore.Mvc;
using static Deadit.Lib.Filter.CommentFilters;

namespace Deadit.WebGui.Controllers.Api;


[ApiController]
[Route("api/communities/{communityName}/posts/{postId:guid}/comments")]
[ServiceFilter(typeof(PostExistsFilter))]
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
    public async Task<IActionResult> GetCommentsAsync([FromRoute] string communityName, [FromRoute] Guid postId, [FromQuery] SortOption? sort)
    {
        SortOption sortOption = sort ?? SortOption.New;

        var getComments = await _viewModelService.GetCommentsApiResponseAsync(postId, ClientId, sortOption);

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
    [ServiceFilter(typeof(CommentSaveFilter))]
    public async Task<ActionResult<ServiceDataResponse<GetCommentDto>>> PutCommentAsync([FromRoute] string communityName, [FromRoute] Guid postId, [FromRoute] Guid commentId, [FromBody] CommentForm commentForm)
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

        return Ok(getComment);
    }

    [HttpDelete("{commentId:guid}")]
    [ActionName(nameof(DeleteCommentAsync))]
    [ServiceFilter(typeof(InternalApiAuthFilter))]
    [ServiceFilter(typeof(CommunityMemberFilter))]
    [ServiceFilter(typeof(CommentDeleteFilter))]
    public async Task<ActionResult<ServiceResponse>> DeleteCommentAsync([FromRoute] string communityName, [FromRoute] Guid postId, [FromRoute] Guid commentId)
    {
        var deleteComment = await _commentService.DeleteCommentAsync(commentId);

        if (!deleteComment.Successful)
        {
            return BadRequest(deleteComment);
        }

        return NoContent();
    }
}
