using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Dto;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Errors;
using Deadit.Lib.Domain.Response;
using Deadit.Lib.Domain.TableView;
using Deadit.Lib.Domain.ViewModel;
using Deadit.Lib.Service.Contracts;

namespace Deadit.Lib.Service.Implementations;


[AutoInject<IViewModelService>(AutoInjectionType.Scoped, InjectionProject.WebGui)]
public class ViewModelService : IViewModelService
{
    private readonly ICommunityService _communityService;
    private readonly ICommunityMemberService _memberService;
    private readonly IAuthService _authService;
    private readonly IPostService _postService;
    private readonly ICommentService _commentService;

    public ViewModelService(ICommunityService communityService, ICommunityMemberService memberService, IAuthService authService, IPostService postService, ICommentService commentService)
    {
        _communityService = communityService;
        _memberService = memberService;
        _authService = authService;
        _postService = postService;
        _commentService = commentService;
    }

    /// <summary>
    /// Get the view model for the CommunityPage
    /// </summary>
    /// <param name="communityName"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundHttpResponseException"></exception>
    public async Task<ServiceDataResponse<CommunityPageViewModel>> GetCommunityPageViewModelAsync(string communityName, uint? userId)
    {
        // get the community
        var getCommunity = await _communityService.GetCommunityAsync(communityName);

        if (!getCommunity.Successful)
        {
            return new(getCommunity);
        }

        if (getCommunity.Data is not ViewCommunity community)
        {
            throw new NotFoundHttpResponseException();
        }

        // get the posts
        var getPosts = await _postService.GetAllBasicPostsAsync(communityName);

        if (!getPosts.Successful)
        {
            return new(getPosts);
        }

        List<ViewPost> posts = getPosts.Data ?? new();
        
        // check if member
        bool isMember = await IsUserCommunityMemberAsync(community.CommunityId, userId);

        CommunityPageViewModel viewModel = new()
        {
            Community = community,
            IsMember = isMember,
            IsLoggedIn = _authService.IsClientLoggedIn(),
            Posts = posts.OrderByDescending(p => p.PostCreatedOn).ToList(),
        };

        return new(viewModel);
    }

    /// <summary>
    /// Check if the user is a member of the specified community
    /// </summary>
    /// <param name="communityId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    private async Task<bool> IsUserCommunityMemberAsync(uint? communityId, uint? userId)
    {
        bool isMember = false;

        if (userId.HasValue && communityId.HasValue)
        {
            isMember = (await _memberService.IsMemberAsync(userId.Value, communityId.Value)).Data;
        }

        return isMember;
    }

    /// <summary>
    /// Get the view model for the joined communities page
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<ServiceDataResponse<JoinedCommunitiesPageViewModel>> GetJoinedCommunitiesPageViewModelAsync(uint userId)
    {
        var serviceResponse = await _memberService.GetJoinedCommunitiesAsync(userId);

        if (!serviceResponse.Successful)
        {
            return new(serviceResponse);
        }

        JoinedCommunitiesPageViewModel viewModel = new()
        {
            Communities = serviceResponse.Data?.Select(m => (ViewCommunity)m)?.OrderBy(c => c.CommunityTitle?.ToLower()).ToList() ?? new(),
        };

        return new(viewModel);
    }


    public async Task<ServiceDataResponse<PostPageViewModel>> GetPostPageViewModelAsync(Guid postId, PostType postType, uint? userId)
    {
        // get the post
        ViewPost post;

        try
        {
            post = postType switch
            {
                PostType.Text => await GetPostTextAsync(postId),
                PostType.Link => await GetPostLinkAsync(postId),
                _             => throw new NotImplementedException(),
            };
        }
        catch(ServiceResponseException ex)
        {
            return new(ex.Response);
        }


        // get comments
        var getComments = await _commentService.GetCommentsNestedAsync(postId);

        if (!getComments.Successful)
        {
            return new(getComments);
        }

        PostPageViewModel viewModel = new()
        {
            Post = post,
            IsAuthor = post.PostAuthorId == userId,
            Comments = getComments.Data ?? new(),
            ClientId = userId,
            IsLoggedIn = userId != null,
        };

        return new(viewModel);
    }


    private async Task<ViewPostText> GetPostTextAsync(Guid postId)
    {
        // get the post
        var getPost = await _postService.GetTextPostAsync(postId);

        if (!getPost.Successful)
        {
            throw new ServiceResponseException(getPost);
        }

        if (getPost.Data is not ViewPostText post)
        {
            throw new NotFoundHttpResponseException();
        }

        return post; 
    }

    private async Task<ViewPostLink> GetPostLinkAsync(Guid postId)
    {
        // get the post
        var getPost = await _postService.GetLinkPostAsync(postId);

        if (!getPost.Successful)
        {
            throw new ServiceResponseException(getPost);
        }

        if (getPost.Data is not ViewPostLink post)
        {
            throw new NotFoundHttpResponseException();
        }

        return post;
    }



    public async Task<ServiceDataResponse<GetCommentsApiViewModel>> GetCommentsApiResponseAsync(Guid postId, uint? clientId)
    {
        var getComments = await _commentService.GetCommentsNestedAsync(postId);

        if (!getComments.Successful)
        {
            return new(getComments);
        }

        var comments = getComments.Data ?? new();

        comments.ForEach(c => c.MaskDeletedInfo());

        var dtos = comments.Select(c =>
        {
            var dto = (GetCommentDto)c;

            dto.SetIsAuthorRecursive(clientId);

            return dto;
        });


        GetCommentsApiViewModel viewModel = new()
        {
            Comments = dtos.ToList(),
            IsLoggedIn = clientId.HasValue,
        };

        return new(viewModel);
    }

    public async Task<ServiceDataResponse<GetCommentDto>> GetCommentApiResponseAsync(Guid commentId, uint? clientId)
    {
        var getComment = await _commentService.GetCommentAsync(commentId);

        if (!getComment.Successful)
        {
            return new(getComment);
        }

        if (getComment.Data is not ViewComment comment)
        {
            throw new NotFoundHttpResponseException();
        }

        var result = (GetCommentDto)comment;
        result.SetIsAuthorRecursive(clientId);

        return new(result);
    }

}

