﻿using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Dto;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Errors;
using Deadit.Lib.Domain.Parms;
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
    private readonly ICommentVotesService _commentVotesService;
    private readonly IPostVotesService _postVotesService;

    public ViewModelService(ICommunityService communityService, ICommunityMemberService memberService, IAuthService authService, IPostService postService, ICommentService commentService, ICommentVotesService commentVotesService, IPostVotesService postVotesService)
    {
        _communityService = communityService;
        _memberService = memberService;
        _authService = authService;
        _postService = postService;
        _commentService = commentService;
        _commentVotesService = commentVotesService;
        _postVotesService = postVotesService;
    }

    /// <summary>
    /// Get the view model for the CommunityPage
    /// </summary>
    /// <param name="communityName"></param>
    /// <param name="clientId"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundHttpResponseException"></exception>
    public async Task<ServiceDataResponse<CommunityPageViewModel>> GetCommunityPageViewModelAsync(string communityName, uint? clientId)
    {
        try
        {
            // get the community information
            var community = await GetCommunityAsync(communityName);

            // get the posts in the community
            List<ViewPost> posts = await GetCommunityPostsAsync(communityName);

            // If client is logged in, get their vote history for the posts
            List<ViewVotePost> userVotes = await GetUserPostVotesInCommunity(clientId, communityName);

            // Combine each post with the client's vote choice (defaults to novote if they have never voted on the post)
            var getPostDtos = posts.BuildGetPostUserVoteDtos(userVotes).OrderByDescending(p => p.Post.PostCreatedOn);

            // check if client community member
            bool isMember = await IsUserCommunityMemberAsync(community.CommunityId, clientId);

            CommunityPageViewModel viewModel = new()
            {
                Community = community,
                IsMember = isMember,
                IsLoggedIn = _authService.IsClientLoggedIn(),
                PostDtos = getPostDtos.ToList(),
            };

            return new(viewModel);
        }
        catch (ServiceResponseException ex)
        {
            return new(ex.Response);
        }

    }


    private async Task<ViewCommunity> GetCommunityAsync(string communityName)
    {
        var getCommunity = await _communityService.GetCommunityAsync(communityName);

        if (!getCommunity.Successful)
        {
            throw new ServiceResponseException(getCommunity);
        }

        var community = NotFoundHttpResponseException.ThrowIfNot<ViewCommunity>(getCommunity.Data);

        return community;
    }

    private async Task<List<ViewPost>> GetCommunityPostsAsync(string communityName)
    {
        var getPosts = await _postService.GetAllBasicPostsAsync(communityName);

        if (!getPosts.Successful)
        {
            throw new ServiceResponseException(getPosts);
        }

        return getPosts.Data ?? new();
    }


    private async Task<List<ViewVotePost>> GetUserPostVotesInCommunity(uint? clientId, string communityName)
    {
        if (clientId is not uint userId)
        {
            return new();
        }

        var getVotes = await _postVotesService.GetUserPostVotesInCommunity(userId, communityName);

        if (!getVotes.Successful)
        {
            throw new ServiceResponseException(getVotes);
        }

        return getVotes.Data ?? new();
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


    public async Task<ServiceDataResponse<PostPageViewModel>> GetPostPageViewModelAsync(Guid postId, PostType postType, uint? userId, SortOption sortBy)
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
        var getComments = await _commentService.GetCommentsNestedAsync(new GetCommentsParms
        {
            PostId = postId,
            Sort = sortBy,
        });

        if (!getComments.Successful)
        {
            return new(getComments);
        }

        // get user's vote selection for the post

        VoteType userPostVote = VoteType.Novote;

        if (userId.HasValue)
        {
            var getPostVote = await _postVotesService.GetVoteAsync(postId, userId.Value);

            if (!getPostVote.Successful)
            {
                return new(getPostVote);
            }

            userPostVote = getPostVote.Data?.VotePostVoteType ?? VoteType.Novote;
            
        }



        PostPageViewModel viewModel = new()
        {
            Post = post,
            IsAuthor = post.PostAuthorId == userId,
            Comments = getComments.Data ?? new(),
            ClientId = userId,
            IsLoggedIn = userId != null,
            SortOption = sortBy,
            UserPostVote = userPostVote,
            
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



    public async Task<ServiceDataResponse<GetCommentsApiViewModel>> GetCommentsApiResponseAsync(Guid postId, uint? clientId, SortOption sort)
    {
        try
        {
            var getComments = await _commentService.GetCommentsNestedAsync(new GetCommentsParms()
            {
                PostId = postId,
                Sort = sort,
            });


            if (!getComments.Successful)
            {
                return new(getComments);
            }

            var comments = getComments.Data ?? new();
            var votes = await GetUserCommentVotesAsync(postId, clientId);
            var dtos = comments.BuildGetCommentDtos(clientId, votes);

            GetCommentsApiViewModel viewModel = new()
            {
                Comments = dtos.ToList(),
                IsLoggedIn = clientId.HasValue,
            };

            return new(viewModel);
        }
        catch(ServiceResponseException ex)
        {
            return new(ex.Response);
        }
    }


    private async Task<List<ViewVoteComment>> GetUserCommentVotesAsync(Guid postId, uint? clientId)
    {

        if (!clientId.HasValue)
        {
            return new();
        }

        var getVotes = await _commentVotesService.GetUserCommentVotesInPost(postId, clientId.Value);

        if (!getVotes.Successful)
        {
            throw new ServiceResponseException(getVotes);
        }

        return getVotes.Data ?? new();
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

