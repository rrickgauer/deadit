using Deadit.Lib.Domain.Forms;
using Deadit.Lib.Domain.Model;
using Deadit.Lib.Domain.Response;
using Deadit.Lib.Domain.TableView;
using Deadit.Lib.Filter;
using Deadit.Lib.Service.Contracts;
using Deadit.WebGui.Controllers.Contracts;
using Deadit.WebGui.Filter;
using Microsoft.AspNetCore.Mvc;

namespace Deadit.WebGui.Controllers.Api;

[ApiController]
[Route("api/flair/posts")]
public class ApiFlairPostsController(IFlairPostService flairPostService) : InternalApiController, IControllerName
{
    public static string ControllerRedirectName => IControllerName.RemoveControllerSuffix(nameof(ApiFlairPostsController));

    private readonly IFlairPostService _flairPostService = flairPostService;

    /// <summary>
    /// GET: /api/flair/posts/:communityName
    /// </summary>
    /// <param name="communityName"></param>
    /// <returns></returns>
    [HttpGet("{communityName}")]
    [ServiceFilter(typeof(CommunityNameExistsFilter))]
    public async Task<IActionResult> GetPostFlairsAsync([FromRoute] string communityName)
    {
        var getFlairs = await _flairPostService.GetFlairPostsAsync(communityName);

        if (!getFlairs.Successful)
        {
            return BadRequest(getFlairs);
        }

        return Ok(getFlairs);
    }

    /// <summary>
    /// GET: /api/flair/posts/:flairId
    /// </summary>
    /// <param name="flairId"></param>
    /// <returns></returns>
    [HttpGet("{flairId:int}")]
    [ServiceFilter(typeof(GetFlairPostFilter))]
    [ActionName(nameof(GetPostFlairAsync))]
    public async Task<IActionResult> GetPostFlairAsync([FromRoute] uint flairId)
    {
        if (RequestItems.FlairPost is not ViewFlairPost flair)
        {
            return NotFound();
        }

        return Ok(new ServiceDataResponse<ViewFlairPost>(flair));
    }

    /// <summary>
    /// POST: /api/flair/posts
    /// </summary>
    /// <param name="form"></param>
    /// <returns></returns>
    [HttpPost]
    [ActionName(nameof(PostFlairPostAsync))]
    [ServiceFilter(typeof(InternalApiAuthFilter))]
    [ServiceFilter(typeof(PostFlairPostFilter))]
    public async Task<IActionResult> PostFlairPostAsync([FromBody] FlairPostForm form)
    {
        FlairPost flair = new()
        {
            Color = form.Color?.ToUpper(),
            CommunityId = RequestItems.CommunityId,
            CreatedOn = DateTime.UtcNow,
            Name = form.Name,
        };

        var createFlair = await _flairPostService.CreateFlairPostAsync(flair);

        if (!createFlair.Successful)
        {
            return BadRequest(createFlair);
        }

        return Ok(createFlair);
    }

    /// <summary>
    /// PUT: /api/flair/posts/:flairId
    /// </summary>
    /// <param name="flairId"></param>
    /// <param name="form"></param>
    /// <returns></returns>
    [HttpPut("{flairId:int}")]
    [ActionName(nameof(PutFlairPostAsync))]
    [ServiceFilter(typeof(InternalApiAuthFilter))]
    [ServiceFilter(typeof(PutFlairPostFilter))]
    public async Task<IActionResult> PutFlairPostAsync([FromRoute] uint flairId, [FromBody] FlairPostForm form)
    {
        FlairPost flair = new()
        {
            Id = flairId,
            Color = form.Color?.ToUpper(),
            CommunityId = RequestItems.CommunityId,
            CreatedOn = DateTime.UtcNow,
            Name = form.Name,
        };

        var createFlair = await _flairPostService.UpdateFlairPostAsync(flair);

        if (!createFlair.Successful)
        {
            return BadRequest(createFlair);
        }

        return Ok(createFlair);
    }

    /// <summary>
    /// DELETE: /api/flair/posts/:flairId
    /// </summary>
    /// <param name="flairId"></param>
    /// <returns></returns>
    [HttpDelete("{flairId:int}")]
    [ActionName(nameof(DeleteFlairPostAsync))]
    [ServiceFilter(typeof(InternalApiAuthFilter))]
    [ServiceFilter(typeof(DeleteFlairPostFilter))]
    public async Task<IActionResult> DeleteFlairPostAsync([FromRoute] uint flairId)
    {
        var delete = await _flairPostService.DeleteFlairPostAsync(flairId);

        if (!delete.Successful)
        {
            return BadRequest(delete);
        }

        return NoContent();
    }

}
