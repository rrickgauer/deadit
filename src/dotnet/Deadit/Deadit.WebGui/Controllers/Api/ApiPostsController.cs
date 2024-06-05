using Deadit.Lib.Domain.Enum;
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
[Route("api/posts")]
public class ApiPostsController(IPostService postService, GetPostApiVMService getPostApiVMService) : InternalApiController, IControllerName
{
    // IControllerName
    public static string ControllerRedirectName => IControllerName.RemoveControllerSuffix(nameof(ApiPostsController));

    private readonly IPostService _postService = postService;
    private readonly GetPostApiVMService _getPostVmService = getPostApiVMService;


    /// <summary>
    /// POST: /api/posts/text
    /// </summary>
    /// <param name="formData"></param>
    /// <returns></returns>
    [HttpPost("text")]
    [ActionName(nameof(CreateTextPostAsync))]
    [ServiceFilter(typeof(InternalApiAuthFilter))]
    [ServiceFilter(typeof(CreatePostFilter))]
    public async Task<IActionResult> CreateTextPostAsync([FromBody] CreateTextPostForm formData)
    {
        var newPostRecord = InitNewPostRecord<PostText>(formData);
        newPostRecord.Content = formData.Content;

        var serviceResponse = await _postService.CreatePostTextAsync(newPostRecord);

        if (!serviceResponse.Successful)
        {
            return BadRequest(serviceResponse);
        }

        if (serviceResponse.Data?.PostId is not Guid postId)
        {
            return NotFound();
        }

        var getPost = await _getPostVmService.GetViewModelAsync(new()
        {
            ClientId = ClientId,
            PostId = postId,
            PostType = PostType.Text,
            SortOption = SortOption.New,
        });

        if (!getPost.Successful)
        {
            return BadRequest(getPost);
        }

        return Created($"", getPost);
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
    /// PUT: /api/posts/text/:postId
    /// </summary>
    /// <param name="postId"></param>
    /// <param name="formData"></param>
    /// <returns></returns>
    [HttpPut("text/{postId:guid}")]
    [ActionName(nameof(PutPostTextAsync))]
    [ServiceFilter(typeof(InternalApiAuthFilter))]
    [ServiceFilter(typeof(ModifyPostFilter))]
    public async Task<IActionResult> PutPostTextAsync([FromRoute] Guid postId, [FromBody] EditPostForm formData)
    {
        PostText post = new()
        {
            Id = postId,
            AuthorId = ClientId,
            CommunityId = RequestItems.CommunityId,
            Content = formData.Content,
        };

        var saveResult = await _postService.SavePostTextAsync(post);

        if (!saveResult.Successful)
        {
            return BadRequest(saveResult);
        }

        return await ReturnBasicPostVm(postId, PostType.Text);
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
    [ServiceFilter(typeof(CreatePostFilter))]
    public async Task<IActionResult> CreateLinkPostAsync([FromBody] CreateLinkPostForm formData)
    {
        var newPostRecord = InitNewPostRecord<PostLink>(formData);
        newPostRecord.Url = formData.Url;

        var savePost = await _postService.CreatePostLinkAsync(newPostRecord);

        if (!savePost.Successful)
        {
            return BadRequest(savePost);
        }

        if (savePost.Data?.PostId is not Guid postId)
        {
            return NotFound();
        }

        var getPost = await _getPostVmService.GetViewModelAsync(new()
        {
            ClientId = ClientId,
            PostId = postId,
            PostType = PostType.Link,
            SortOption = SortOption.New,
        });

        if (!getPost.Successful)
        {
            return BadRequest(getPost);
        }

        return Created($"", getPost);
    }


    private async Task<IActionResult> ReturnBasicPostVm(Guid postId, PostType postType)
    {
        var getPost = await _getPostVmService.GetViewModelAsync(new()
        {
            ClientId = ClientId,
            PostId = postId,
            PostType = postType,
            SortOption = SortOption.New,
        });

        if (!getPost.Successful)
        {
            return BadRequest(getPost);
        }

        return Ok(getPost);
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
    public async Task<IActionResult> DeletePostAsync([FromRoute] Guid postId)
    {
        var deletePost = await _postService.AuthorDeletePostAsync(postId);

        if (!deletePost.Successful)
        {
            return BadRequest(deletePost);
        }

        return NoContent();
    }


    /// <summary>
    /// GET: /api/posts/:postId
    /// </summary>
    /// <param name="postId"></param>
    /// <param name="sort"></param>
    /// <returns></returns>
    [HttpGet("{postId:guid}")]
    [ActionName(nameof(GetPostAsync))]
    [ServiceFilter(typeof(PostExistsFilter))]
    public async Task<IActionResult> GetPostAsync([FromRoute] Guid postId, [FromQuery] SortOption? sort)
    {
        SortOption sortOption = sort ?? SortOption.New;

        var getViewModel = await _getPostVmService.GetViewModelAsync(new()
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

    /// <summary>
    /// PATCH: /api/posts/:postId
    /// </summary>
    /// <param name="postId"></param>
    /// <param name="moderateForm"></param>
    /// <returns></returns>
    [HttpPatch("{postId:guid}")]
    [ActionName(nameof(ModeratePostAsync))]
    [ServiceFilter(typeof(InternalApiAuthFilter))]
    [ServiceFilter(typeof(ModeratePostFilter))]
    public async Task<IActionResult> ModeratePostAsync([FromRoute] Guid postId, [FromBody] ModeratePostForm? moderateForm)
    {
        var result = await _postService.ModeratePostAsync(postId, moderateForm ?? new());

        if (!result.Successful)
        {
            return BadRequest(result);
        }

        var getPost = await _getPostVmService.GetViewModelAsync(new()
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
