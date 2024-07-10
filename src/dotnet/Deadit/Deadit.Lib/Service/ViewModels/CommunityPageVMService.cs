using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Dto;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Errors;
using Deadit.Lib.Domain.Other;
using Deadit.Lib.Domain.Parms;
using Deadit.Lib.Domain.Response;
using Deadit.Lib.Domain.TableView;
using Deadit.Lib.Domain.ViewModel;
using Deadit.Lib.Service.Contracts;

namespace Deadit.Lib.Service.ViewModels;


[AutoInject(AutoInjectionType.Scoped, InjectionProject.WebGui)]
public class CommunityPageVMService : IVMService<CommunityPageViewModelParms, CommunityPageViewModel>
{
    private readonly ICommunityService _communityService;
    private readonly ICommunityMemberService _memberService;
    private readonly IAuthService _authService;
    private readonly IPostService _postService;
    private readonly IPostVotesService _postVotesService;

    public CommunityPageVMService(ICommunityService communityService, ICommunityMemberService memberService, IAuthService authService, IPostService postService, IPostVotesService postVotesService)
    {
        _communityService = communityService;
        _memberService = memberService;
        _authService = authService;
        _postService = postService;
        _postVotesService = postVotesService;
    }

    public async Task<ServiceDataResponse<CommunityPageViewModel>> GetViewModelAsync(CommunityPageViewModelParms args)
    {
        try
        {
            // get the community information
            var community = await GetCommunityAsync(args.CommunityName);

            // get the posts in the community
            List<ViewPost> posts = await GetCommunityPostsAsync(args);

            // If client is logged in, get their vote history for the posts
            List<ViewVotePost> userVotes = await GetUserPostVotesInCommunity(args);

            // Combine each post with the client's vote choice (defaults to novote if they have never voted on the post)
            var getPostDtos = posts.BuildGetPostUserVoteDtos(userVotes);

            // check if client community member
            bool isMember = await IsUserCommunityMemberAsync(community.CommunityId, args.ClientId);

            CommunityPageViewModel viewModel = new()
            {
                Community = community,
                IsMember = isMember,
                IsLoggedIn = _authService.IsClientLoggedIn(),
                PostDtos = getPostDtos.ToList(),
                PostSort = args.PostSorting,
                Pagination = args.Pagination,
                IsModerator = args.ClientId == community.CommunityOwnerId,
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

        // return 404 if we couldn't find the community
        return NotFoundHttpResponseException.ThrowIfNot<ViewCommunity>(getCommunity.Data);
    }


    private async Task<List<ViewPost>> GetCommunityPostsAsync(CommunityPageViewModelParms args)
    {
        List<ViewPost> posts = new();

        // order the posts appropriately
        if (args.PostSorting.SortByNew)
        {
            posts = await GetNewestCommunityPostsAsync(args);
        }
        else
        {
            posts = await GetTopCommunityPostsAsync(args);
        }

        posts.ForEach(p => p.HandlePostDeleted());

        return posts;
    }


    private async Task<List<ViewPost>> GetNewestCommunityPostsAsync(CommunityPageViewModelParms args)
    {
        ServiceDataResponse<List<ViewPost>> getPosts;

        if (args.FilterByFlairId is uint flairId)
        {
            getPosts = await _postService.GetNewestCommunityPostsAsync(args.CommunityName, args.Pagination, flairId);
        }
        else
        {
            getPosts = await _postService.GetNewestCommunityPostsAsync(args.CommunityName, args.Pagination);
        }

        if (!getPosts.Successful)
        {
            throw new ServiceResponseException(getPosts);
        }

        return getPosts.Data ?? new();
    }

    private async Task<List<ViewPost>> GetTopCommunityPostsAsync(CommunityPageViewModelParms args)
    {
        ServiceDataResponse<List<ViewPost>> getPosts;

        if (args.FilterByFlairId is uint flairId)
        {
            getPosts = await _postService.GetTopCommunityPostsAsync(args.CommunityName, args.PostSorting.TopSort, args.Pagination, flairId);
        }
        else
        {
            getPosts = await _postService.GetTopCommunityPostsAsync(args.CommunityName, args.PostSorting.TopSort, args.Pagination);
        }


        if (!getPosts.Successful)
        {
            throw new ServiceResponseException(getPosts);
        }

        return getPosts.Data ?? new();
    }


    private async Task<List<ViewVotePost>> GetUserPostVotesInCommunity(CommunityPageViewModelParms args)
    {
        if (args.ClientId is not uint userId)
        {
            return new();
        }

        var getVotes = await _postVotesService.GetUserPostVotesInCommunity(userId, args.CommunityName);

        if (!getVotes.Successful)
        {
            throw new ServiceResponseException(getVotes);
        }

        return getVotes.Data ?? new();
    }


    private async Task<bool> IsUserCommunityMemberAsync(uint? communityId, uint? userId)
    {
        bool isMember = false;

        if (userId.HasValue && communityId.HasValue)
        {
            isMember = (await _memberService.IsMemberAsync(userId.Value, communityId.Value)).Data;
        }

        return isMember;
    }
}
