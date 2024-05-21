using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Dto;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Errors;
using Deadit.Lib.Domain.Parms;
using Deadit.Lib.Domain.Response;
using Deadit.Lib.Domain.TableView;
using Deadit.Lib.Domain.ViewModel;
using Deadit.Lib.Service.Contracts;

namespace Deadit.Lib.Service.ViewModels;

[AutoInject(AutoInjectionType.Scoped, InjectionProject.WebGui)]
public class HomePageVMService(IPostService postService, IPostVotesService postVoteService) : IVMService<GetHomePageParms, HomePageViewModel>
{
    private readonly IPostService _postService = postService;
    private readonly IPostVotesService _postVoteService = postVoteService;

    public async Task<ServiceDataResponse<HomePageViewModel>> GetViewModelAsync(GetHomePageParms args)
    {

        try
        {
            var posts = await GetPostDtosAsync(args);

            HomePageViewModel viewModel = new()
            {
                ClientId = args.ClientId,
                Posts = posts,
                Pagination = args.Pagination,
                PostSort = args.PostSorting,
            };

            return new(viewModel);
        }

        catch (ServiceResponseException ex)
        {
            return ex;
        }


    }



    private async Task<List<GetPostUserVoteDto>> GetPostDtosAsync(GetHomePageParms args)
    {

        if (args.ClientId is not uint clientId)
        {
            return new();
        }


        var postItems = await GetPostsAsync(args);

        var postIds = postItems.Select(p => p.PostId!.Value);
        var getVotes = await _postVoteService.GetUserPostVotesAsync(clientId, postIds);

        if (!getVotes.Successful)
        {
            throw new ServiceResponseException(getVotes);
        }

        var votes = getVotes.Data ?? new();

        var getPostDtos = postItems.BuildGetPostUserVoteDtos(votes);

        return new(getPostDtos);
    }

    private async Task<List<ViewPost>> GetPostsAsync(GetHomePageParms args)
    {
        if (args.ClientId is not uint clientId)
        {
            return new();
        }

        ServiceDataResponse<List<ViewPost>> getPosts = new();

        getPosts = args.PostSorting.PostSortType switch
        {
            PostSortType.New => await _postService.GetUserNewHomeFeedAsycn(clientId, args.Pagination),
            PostSortType.Top => await _postService.GetUserTopHomeFeedAsync(clientId, args.Pagination, args.PostSorting.TopSort),
            _                => throw new NotImplementedException(),
        };


        if (!getPosts.Successful)
        {
            throw new ServiceResponseException(getPosts);
        }

        var posts = getPosts.Data ?? new();

        return posts;
    }
}