using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Errors;
using Deadit.Lib.Filter;
using Deadit.Lib.Service.Contracts;
using Deadit.WebGui.Controllers.Contracts;
using Deadit.WebGui.Filter;
using Microsoft.AspNetCore.Mvc;
using static Deadit.Lib.Domain.Model.Votes;

namespace Deadit.WebGui.Controllers.Api;

[ApiController]
[Route("api/votes")]
public class ApiVotesController(IPostVotesService votesService) : InternalApiController, IControllerName
{
    // IControllerName
    public static string ControllerRedirectName => IControllerName.RemoveControllerSuffix(nameof(ApiVotesController));

    private readonly IPostVotesService _votesService = votesService;

    [HttpPut("posts/{postId:guid}/{voteType}")]
    [ActionName(nameof(PutPostVoteAsync))]
    [ServiceFilter(typeof(InternalApiAuthFilter))]
    [ServiceFilter(typeof(GetPostFilter))]
    public async Task<IActionResult> PutPostVoteAsync([FromRoute] Guid postId, [FromRoute] VoteType voteType)
    {
        try
        {
            VotePost newVote = new()
            {
                CreatedOn = DateTime.UtcNow,
                ItemId = postId,
                VoteType = voteType,
                UserId = ClientId!.Value,
            };

            var saveResult = await _votesService.SaveVoteAsync(newVote);

            if (!saveResult.Successful)
            {
                return BadRequest(saveResult);  
            }

            return Ok(saveResult);
        }
        catch(ServiceResponseException ex)
        {
            return BadRequest(ex.Response);
        }
    }


    [HttpGet("posts/{postId:guid}")]
    [ActionName(nameof(GetPostVotesAsync))]
    [ServiceFilter(typeof(GetPostFilter))]
    public async Task<IActionResult> GetPostVotesAsync([FromRoute] Guid postId)
    {
        try
        {
            var getVotes = await _votesService.GetPostVotesAsync(postId);
            
            if (!getVotes.Successful)
            {
                return BadRequest(getVotes);
            }

            return Ok(getVotes);
        }
        catch(ServiceResponseException ex)
        {
            return BadRequest(ex.Response);
        }
    }
}
