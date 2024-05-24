using Deadit.Lib.Domain.Errors;
using Deadit.Lib.Domain.Forms;
using Deadit.Lib.Domain.Model;
using Deadit.Lib.Filter;
using Deadit.Lib.Service.Contracts;
using Deadit.WebGui.Controllers.Contracts;
using Deadit.WebGui.Filter;
using Microsoft.AspNetCore.Mvc;
using static Deadit.Lib.Domain.Forms.CreatePostForms;

namespace Deadit.WebGui.Controllers.Api;


[ApiController]
[Route("api/communities/{communityName}/posts")]
[ServiceFilter(typeof(CommunityNameExistsFilter))]
public class ApiCommunityPostsController(IPostService postService) : InternalApiController, IControllerName
{
    // IControllerName
    public static string ControllerRedirectName => IControllerName.RemoveControllerSuffix(nameof(ApiCommunityPostsController));

    private readonly IPostService _postService = postService;


    /// <summary>
    /// POST: /api/communities/:communityName/posts/text
    /// </summary>
    /// <param name="communityName"></param>
    /// <param name="formData"></param>
    /// <returns></returns>
    [HttpPost("text")]
    [ActionName(nameof(CreateTextPostAsync))]
    [ServiceFilter(typeof(InternalApiAuthFilter))]
    [ServiceFilter(typeof(CommunityMemberFilter))]
    public async Task<IActionResult> CreateTextPostAsync([FromRoute] string communityName, [FromBody] CreateTextPostForm formData)
    {
        var newPostRecord = InitNewPostRecord<PostText>(formData);
        newPostRecord.Content = formData.Content;   

        var serviceResponse = await _postService.CreatePostTextAsync(newPostRecord);

        if (!serviceResponse.Successful)
        {
            return BadRequest(serviceResponse);
        }

        return Created($"", serviceResponse);
    }


    [HttpPut("text/{postId:guid}")]
    [ActionName(nameof(PutPostTextAsync))]
    [ServiceFilter(typeof(InternalApiAuthFilter))]
    [ServiceFilter(typeof(ModifyPostFilter))]
    public async Task<IActionResult> PutPostTextAsync([FromRoute] string communityName, [FromRoute] Guid postId, [FromBody] EditPostForm formData)
    {
        if (RequestItems.CommunityId is not uint communityId)
        {
            return NotFound();
        }

        PostText post = new()
        {
            Id = postId,
            AuthorId = ClientId,
            CommunityId = communityId,
            Content = formData.Content,
        };

        try
        {
            var saveResult = await _postService.SavePostTextAsync(post);

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



    /// <summary>
    /// POST: /api/communities/:communityName/posts/link
    /// </summary>
    /// <param name="communityName"></param>
    /// <param name="formData"></param>
    /// <returns></returns>
    [HttpPost("link")]
    [ActionName(nameof(CreateLinkPostAsync))]
    [ServiceFilter(typeof(InternalApiAuthFilter))]
    [ServiceFilter(typeof(CommunityMemberFilter))]
    public async Task<IActionResult> CreateLinkPostAsync([FromRoute] string communityName, [FromBody] CreateLinkPostForm formData)
    {
        var newPostRecord = InitNewPostRecord<PostLink>(formData);
        newPostRecord.Url = formData.Url;

        var serviceResponse = await _postService.CreatePostLinkAsync(newPostRecord);

        if (!serviceResponse.Successful)
        {
            return BadRequest(serviceResponse);
        }

        return Created($"", serviceResponse);
    }


    private T InitNewPostRecord<T>(CreatePostForm formData) where T : Post, new()
    {
        T post = new()
        {
            CommunityId = RequestItems.CommunityId,
            AuthorId    = ClientId!.Value,
            CreatedOn   = DateTime.UtcNow,
            Id          = Guid.NewGuid(),
            Title       = formData.Title,
        };

        return post;
    }


    [HttpDelete("{postId:guid}")]
    [ActionName(nameof(DeletePostAsync))]
    [ServiceFilter(typeof(InternalApiAuthFilter))]
    [ServiceFilter(typeof(ModifyPostFilter))]
    public async Task<IActionResult> DeletePostAsync([FromRoute] string communityName, [FromRoute] Guid postId)
    {
        try
        {
            var deletePost = await _postService.AuthorDeletePostAsync(postId);

            if (!deletePost.Successful)
            {
                return BadRequest(deletePost);
            }

            return NoContent();

        }
        catch(ServiceResponseException ex)
        {
            return BadRequest(ex.Response);
        }
    }
}
