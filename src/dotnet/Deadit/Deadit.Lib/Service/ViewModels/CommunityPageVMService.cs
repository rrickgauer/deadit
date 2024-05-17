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
            List<ViewPost> posts = new();

            if (args.PostSorting.SortByNew)
            {
                posts = await GetNewestCommunityPostsAsync(args.CommunityName);
            }
            else
            {
                posts = await GetTopCommunityPostsAsync(args.CommunityName, args.PostSorting.TopSort);
            }

            // If client is logged in, get their vote history for the posts
            List<ViewVotePost> userVotes = await GetUserPostVotesInCommunity(args.ClientId, args.CommunityName);

            // Combine each post with the client's vote choice (defaults to novote if they have never voted on the post)
            //var getPostDtos = posts.BuildGetPostUserVoteDtos(userVotes).OrderByDescending(p => p.Post.PostCreatedOn);
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

        return NotFoundHttpResponseException.ThrowIfNot<ViewCommunity>(getCommunity.Data);
    }

    private async Task<List<ViewPost>> GetNewestCommunityPostsAsync(string communityName)
    {
        var getPosts = await _postService.GetAllBasicPostsAsync(communityName);

        if (!getPosts.Successful)
        {
            throw new ServiceResponseException(getPosts);
        }

        return getPosts.Data ?? new();
    }

    private async Task<List<ViewPost>> GetTopCommunityPostsAsync(string communityName, TopPostSort topSort)
    {
        var getPosts = await _postService.GetTopCommunityPostsAsync(communityName, topSort);

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
}
