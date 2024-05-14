using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Service.Contracts;
using Deadit.WebGui.Controllers.Contracts;
using Deadit.WebGui.Filter;
using Microsoft.AspNetCore.Mvc;
using static Deadit.Lib.Domain.Model.Votes;

namespace Deadit.WebGui.Controllers.Api;

[ApiController]
[Route("api/communities/{communityName}/posts/{postId:guid}/comments/{commentId:guid}/votes")]
public class ApiCommentVotesController(ICommentVotesService voteService) : InternalApiController, IControllerName
{
    // IControllerName
    public static string ControllerRedirectName => IControllerName.RemoveControllerSuffix(nameof(ApiCommentVotesController));


    private readonly ICommentVotesService _voteService = voteService;

    [HttpGet]
    [ActionName(nameof(GetCommentVotesAsync))]
    [ServiceFilter(typeof(InternalApiAuthFilter))]
    public async Task<IActionResult> GetCommentVotesAsync([FromRoute] string communityName, [FromRoute] Guid postId, [FromRoute] Guid commentId)
    {
        var getVotes = await _voteService.GetVoteCommentsAsync(commentId);
        //var getVotes = await _voteService.GetUserCommentVotesInPost(postId, ClientId!.Value);

        if (!getVotes.Successful)
        {
            return BadRequest(getVotes);
        }

        return Ok(getVotes);
    }



    [HttpPut("{voteType}")]
    [ActionName(nameof(PutCommentVoteAsync))]
    [ServiceFilter(typeof(InternalApiAuthFilter))]
    public async Task<IActionResult> PutCommentVoteAsync([FromRoute] string communityName, [FromRoute] Guid postId, [FromRoute] Guid commentId, [FromRoute] VoteType voteType)
    {
        VoteComment vote = new()
        {
            CreatedOn = DateTime.UtcNow,
            ItemId = commentId,
            UserId = ClientId!.Value,
            VoteType = voteType,
        };

        var saveResult = await _voteService.SaveVoteAsync(vote);

        if (!saveResult.Successful)
        {
            return BadRequest(saveResult);
        }


        var getVote = await _voteService.GetVoteAsync(commentId, ClientId!.Value);

        if (!getVote.Successful)
        {
            return BadRequest(getVote);
        }

        return Ok(getVote);
    }




}
