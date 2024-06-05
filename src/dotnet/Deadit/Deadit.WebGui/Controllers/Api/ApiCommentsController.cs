using Deadit.Lib.Domain.Dto;
using Deadit.Lib.Domain.Forms;
using Deadit.Lib.Domain.Model;
using Deadit.Lib.Domain.Response;
using Deadit.Lib.Domain.TableView;
using Deadit.Lib.Filter;
using Deadit.Lib.Service.Contracts;
using Deadit.Lib.Service.ViewModels;
using Deadit.WebGui.Controllers.Contracts;
using Deadit.WebGui.Filter;
using Microsoft.AspNetCore.Mvc;
using static Deadit.Lib.Filter.CommentFilters;

namespace Deadit.WebGui.Controllers.Api;

[ApiController]
[Route("api/comments")]
public class ApiCommentsController(ICommentService commentService, GetCommentDtoVMService getCommentDtoVMService) : InternalApiController, IControllerName
{
    // IControllerName
    public static string ControllerRedirectName => IControllerName.RemoveControllerSuffix(nameof(ApiCommentsController));

    private readonly ICommentService _commentService = commentService;
    private readonly GetCommentDtoVMService _getCommentDtoVMService = getCommentDtoVMService;


    /// <summary>
    /// PUT: /api/comments/:commentId
    /// </summary>
    /// <param name="commentId"></param>
    /// <param name="commentForm"></param>
    /// <returns></returns>
    [HttpPut("{commentId:guid}")]
    [ActionName(nameof(PutCommentAsync))]
    [ServiceFilter(typeof(InternalApiAuthFilter))]
    [ServiceFilter(typeof(CommentSaveFilter))]
    public async Task<ActionResult<ServiceDataResponse<GetCommentDto>>> PutCommentAsync([FromRoute] Guid commentId, [FromBody] CommentForm commentForm)
    {
        Comment comment = new()
        {
            PostId = commentForm.PostId,
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

        var getComment = await _getCommentDtoVMService.GetViewModelAsync(new()
        {
            ClientId = ClientId,
            CommentId = commentId,
        });

        if (!getComment.Successful)
        {
            return BadRequest(getComment);
        }

        return Ok(getComment);
    }


    /// <summary>
    /// GET: /api/comments/:commentId
    /// </summary>
    /// <param name="commentId"></param>
    /// <returns></returns>
    [HttpGet("{commentId:guid}")]
    [ActionName(nameof(GetCommentAsync))]
    [ServiceFilter(typeof(CommentExistsFilter))]
    public async Task<IActionResult> GetCommentAsync([FromRoute] Guid commentId)
    {
        var getComment = await _getCommentDtoVMService.GetViewModelAsync(new()
        {
            ClientId = ClientId,
            CommentId = commentId,
        });


        if (!getComment.Successful)
        {
            return BadRequest(getComment);
        }

        return Ok(getComment);
    }


    /// <summary>
    /// DELETE: /api/comments/:commentId
    /// </summary>
    /// <param name="commentId"></param>
    /// <returns></returns>
    [HttpDelete("{commentId:guid}")]
    [ActionName(nameof(DeleteCommentAsync))]
    [ServiceFilter(typeof(InternalApiAuthFilter))]
    [ServiceFilter(typeof(CommentDeleteFilter))]
    public async Task<ActionResult<ServiceResponse>> DeleteCommentAsync([FromRoute] Guid commentId)
    {
        var deleteComment = await _commentService.DeleteCommentAsync(commentId);

        if (!deleteComment.Successful)
        {
            return BadRequest(deleteComment);
        }

        return NoContent();
    }

    /// <summary>
    /// PATCH: /api/comments/:commentId
    /// </summary>
    /// <param name="commentId"></param>
    /// <param name="patchForm"></param>
    /// <returns></returns>
    [HttpPatch("{commentId:guid}")]
    [ActionName(nameof(ModerateCommentAsync))]
    [ServiceFilter(typeof(InternalApiAuthFilter))]
    [ServiceFilter(typeof(ModerateCommentFilter))]
    public async Task<IActionResult> ModerateCommentAsync([FromRoute] Guid commentId, [FromBody] ModerateCommentForm? patchForm)
    {
        var getComment = await _commentService.GetCommentAsync(commentId);

        if (!getComment.Successful)
        {
            return BadRequest(getComment);
        }

        if (getComment.Data is not ViewComment viewComment)
        {
            return NotFound();
        }

        viewComment.SetPatchFields(patchForm ?? new());


        var saveResult = await _commentService.SaveModerateCommentAsync((Comment)viewComment);

        if (!saveResult.Successful)
        {
            return BadRequest(saveResult);
        }


        var getCommentViewModel = await _getCommentDtoVMService.GetViewModelAsync(new()
        {
            ClientId = ClientId,
            CommentId = commentId,
        });


        if (!getCommentViewModel.Successful)
        {
            return BadRequest(getCommentViewModel);
        }

        return Ok(getCommentViewModel);
    }



}
