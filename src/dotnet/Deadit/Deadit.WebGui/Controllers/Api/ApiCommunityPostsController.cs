using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Errors;
using Deadit.Lib.Domain.Forms;
using Deadit.Lib.Domain.Model;
using Deadit.Lib.Filter;
using Deadit.Lib.Service.Contracts;
using Deadit.Lib.Service.ViewModels;
using Deadit.WebGui.Controllers.Contracts;
using Deadit.WebGui.Filter;
using Microsoft.AspNetCore.Mvc;
using static Deadit.Lib.Domain.Forms.CreatePostForms;

namespace Deadit.WebGui.Controllers.Api;


[ApiController]
[Route("api/communities/{communityName}/posts")]
[ServiceFilter(typeof(CommunityNameExistsFilter))]
public class ApiCommunityPostsController(IPostService postService, GetPostApiVMService getPostApiVMService) : InternalApiController, IControllerName
{
    // IControllerName
    public static string ControllerRedirectName => IControllerName.RemoveControllerSuffix(nameof(ApiCommunityPostsController));

    private readonly IPostService _postService = postService;

    private readonly GetPostApiVMService _getPostApiVMService = getPostApiVMService;


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

    /// <summary>
    /// PUT: /api/communities/:communityName/posts/text/:postId
    /// </summary>
    /// <param name="communityName"></param>
    /// <param name="postId"></param>
    /// <param name="formData"></param>
    /// <returns></returns>
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
        catch (ServiceResponseException ex)
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
            AuthorId = ClientId!.Value,
            CreatedOn = DateTime.UtcNow,
            Id = Guid.NewGuid(),
            Title = formData.Title,
        };

        return post;
    }

    /// <summary>
    /// DELETE: /api/communities/:communityId/posts/:postId
    /// </summary>
    /// <param name="communityName"></param>
    /// <param name="postId"></param>
    /// <returns></returns>
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
        catch (ServiceResponseException ex)
        {
            return BadRequest(ex.Response);
        }
    }

    /// <summary>
    /// GET: /api/communities/:communityName/posts/:postId
    /// </summary>
    /// <param name="communityName"></param>
    /// <param name="postId"></param>
    /// <param name="sort"></param>
    /// <returns></returns>
    [HttpGet("{postId:guid}")]
    [ActionName(nameof(GetPostAsync))]
    [ServiceFilter(typeof(PostExistsFilter))]
    public async Task<IActionResult> GetPostAsync([FromRoute] string communityName, [FromRoute] Guid postId, [FromQuery] SortOption? sort)
    {
        SortOption sortOption = sort ?? SortOption.New;

        var getViewModel = await _getPostApiVMService.GetViewModelAsync(new()
        {
            ClientId = ClientId,
            PostId = postId,
            PostType = RequestItems.Post!.PostType!.Value,
            SortOption = sortOption,
        });


        if (!getViewModel.Successful)
        {
            return BadRequest(getViewModel);
        }

        return Ok(getViewModel);
    }


    [HttpPatch("{postId:guid}")]
    [ActionName(nameof(ModeratePostAsync))]
    [ServiceFilter(typeof(InternalApiAuthFilter))]
    [ServiceFilter(typeof(ModeratePostFilter))]
    public async Task<IActionResult> ModeratePostAsync([FromRoute] string communityName, [FromRoute] Guid postId, [FromBody] ModeratePostForm? moderateForm)
    {
        var result = await _postService.ModeratePostAsync(postId, moderateForm ?? new());

        if (!result.Successful)
        {
            return BadRequest(result);
        }

        var getPost = await _getPostApiVMService.GetViewModelAsync(new()
        {
            ClientId = ClientId,
            PostId = postId,
            PostType = RequestItems.Post!.PostType!.Value,
            SortOption = SortOption.New,
        });

        if (!getPost.Successful)
        { 
            return BadRequest(getPost);
        }

        return Ok(getPost);
    }




}
