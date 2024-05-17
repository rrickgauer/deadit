using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Errors;
using Deadit.Lib.Filter;
using Deadit.Lib.Service.Contracts;
using Deadit.WebGui.Controllers.Contracts;
using Deadit.WebGui.Filter;
using Microsoft.AspNetCore.Mvc;
using static Deadit.Lib.Domain.Model.Votes;
using static Deadit.Lib.Filter.CommentFilters;

namespace Deadit.WebGui.Controllers.Api;


[ApiController]
[Route("api/votes")]
public class ApiVotesController(IPostVotesService postVotesService, ICommentVotesService commentVotesService) : InternalApiController, IControllerName
{

    #region - Private Members - 

    // IControllerName
    public static string ControllerRedirectName => IControllerName.RemoveControllerSuffix(nameof(ApiVotesController));

    private readonly IPostVotesService _postVotesService = postVotesService;
    private readonly ICommentVotesService _commentVotesService = commentVotesService;

    #endregion

    #region - Posts -

    /// <summary>
    /// GET: /api/votes/posts/:postId
    /// </summary>
    /// <param name="postId"></param>
    /// <returns></returns>
    [HttpGet("posts/{postId:guid}")]
    [ActionName(nameof(GetPostVotesAsync))]
    [ServiceFilter(typeof(PostExistsFilter))]
    public async Task<IActionResult> GetPostVotesAsync([FromRoute] Guid postId)
    {
        try
        {
            var getVotes = await _postVotesService.GetPostVotesAsync(postId);

            if (!getVotes.Successful)
            {
                return BadRequest(getVotes);
            }

            return Ok(getVotes);
        }
        catch (ServiceResponseException ex)
        {
            return BadRequest(ex.Response);
        }
    }

    /// <summary>
    /// PUT: /api/votes/posts/:postId/:voteType
    /// </summary>
    /// <param name="postId"></param>
    /// <param name="voteType"></param>
    /// <returns></returns>
    [HttpPut("posts/{postId:guid}/{voteType}")]
    [ActionName(nameof(PutPostVoteAsync))]
    [ServiceFilter(typeof(InternalApiAuthFilter))]
    [ServiceFilter(typeof(PostExistsFilter))]
    public async Task<IActionResult> PutPostVoteAsync([FromRoute] Guid postId, [FromRoute] VoteType voteType)
    {
        try
        {
            var newVote = CreateNewVote<VotePost>(voteType, postId);

            var saveResult = await _postVotesService.SaveVoteAsync(newVote);

            if (!saveResult.Successful)
            {
                return BadRequest(saveResult);
            }

            return Ok(saveResult);
        }
        catch (ServiceResponseException ex)
        {
            return BadRequest(ex.Response);
        }
    }

    #endregion

    #region - Comments -

    /// <summary>
    /// GET: /api/votes/comments/:commentId
    /// </summary>
    /// <param name="commentId"></param>
    /// <returns></returns>
    [HttpGet("comments/{commentId:guid}")]
    [ActionName(nameof(GetCommentVotesAsync))]
    [ServiceFilter(typeof(CommentExistsFilter))]
    public async Task<IActionResult> GetCommentVotesAsync([FromRoute] Guid commentId)
    {
        try
        {
            var getVotes = await _commentVotesService.GetVoteCommentsAsync(commentId);

            if (!getVotes.Successful)
            {
                return BadRequest(getVotes);
            }

            return Ok(getVotes);
        }
        catch (ServiceResponseException ex)
        {
            return BadRequest(ex.Response);
        }
    }


    /// <summary>
    /// PUT: /api/votes/comments/:commentId/:voteType
    /// </summary>
    /// <param name="commentId"></param>
    /// <param name="voteType"></param>
    /// <returns></returns>
    [HttpPut("comments/{commentId:guid}/{voteType}")]
    [ActionName(nameof(PutCommentVoteAsync))]
    [ServiceFilter(typeof(InternalApiAuthFilter))]
    [ServiceFilter(typeof(CommentExistsFilter))]
    public async Task<IActionResult> PutCommentVoteAsync([FromRoute] Guid commentId, [FromRoute] VoteType voteType)
    {
        try
        {
            var newVote = CreateNewVote<VoteComment>(voteType, commentId);

            var saveResult = await _commentVotesService.SaveVoteAsync(newVote);

            if (!saveResult.Successful)
            {
                return BadRequest(saveResult);
            }

            var getVote = await _commentVotesService.GetVoteAsync(commentId, ClientId!.Value);

            if (!getVote.Successful)
            {
                return BadRequest(getVote);
            }

            return Ok(getVote);
        }
        catch (ServiceResponseException ex)
        {
            return BadRequest(ex.Response);
        }
    }


    #endregion

    #region - Private Methods -

    private T CreateNewVote<T>(VoteType voteType, Guid itemId) where T : Vote, new()
    {
        if (ClientId is not uint clientId)
        {
            throw new ForbiddenHttpResponseException();
        }

        T vote = new()
        {
            ItemId    = itemId,
            VoteType  = voteType,
            CreatedOn = DateTime.UtcNow,
            UserId    = clientId,
        };

        return vote;

    }

    #endregion
}
