using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Errors;
using Deadit.Lib.Domain.Parms;
using Deadit.Lib.Domain.Response;
using Deadit.Lib.Domain.TableView;
using Deadit.Lib.Domain.ViewModel;
using Deadit.Lib.Service.Contracts;

namespace Deadit.Lib.Service.ViewModels;

[AutoInject(AutoInjectionType.Scoped, InjectionProject.WebGui)]
public class NewPostPageVMService(ICommunityService communityService, IFlairPostService flairService) : IVMService<NewPostPageVMServiceParms, NewPostPageViewModel>
{
    private readonly ICommunityService _communityService = communityService;
    private readonly IFlairPostService _flairService = flairService;

    public async Task<ServiceDataResponse<NewPostPageViewModel>> GetViewModelAsync(NewPostPageVMServiceParms args)
    {
        try
        {
            var community = await GetCommunityAsync(args);
            var flairs = await GetFlairPostsAsync(args);

            NewPostPageViewModel viewModel = new()
            {
                Community = community,
                FlairPosts = flairs,
            };

            return viewModel;
        }
        catch (ServiceResponseException ex)
        {
            return ex;
        }

    }

    private async Task<ViewCommunity> GetCommunityAsync(NewPostPageVMServiceParms args)
    {
        var getCommunity = await _communityService.GetCommunityAsync(args.CommunityName);

        getCommunity.ThrowIfError();

        if (getCommunity.Data is not ViewCommunity community)
        {
            throw new NotFoundHttpResponseException();
        }

        return community;
    }

    private async Task<List<ViewFlairPost>> GetFlairPostsAsync(NewPostPageVMServiceParms args)
    {
        var getFlairs = await _flairService.GetFlairPostsAsync(args.CommunityName);

        getFlairs.ThrowIfError();

        return getFlairs.Data ?? new();
    }
}
