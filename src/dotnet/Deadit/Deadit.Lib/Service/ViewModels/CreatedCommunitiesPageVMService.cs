using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Parms;
using Deadit.Lib.Domain.Response;
using Deadit.Lib.Domain.ViewModel;
using Deadit.Lib.Service.Contracts;

namespace Deadit.Lib.Service.ViewModels;

[AutoInject(AutoInjectionType.Scoped, InjectionProject.WebGui)]
public class CreatedCommunitiesPageVMService(ICommunityService communitiesService) : IVMService<CreatedCommunitiesPageParms, CommunitiesPageViewModel>
{
    private readonly ICommunityService _communitiesService = communitiesService;

    public async Task<ServiceDataResponse<CommunitiesPageViewModel>> GetViewModelAsync(CreatedCommunitiesPageParms args)
    {
        var getCommunities = await _communitiesService.GetCreatedCommunitiesAsync(args.UserId);

        if (!getCommunities.Successful)
        {
            return new(getCommunities);
        }

        var communities = getCommunities.Data ?? new();

        CommunitiesPageViewModel viewModel = new()
        {
            Communities = communities,
        };

        return new(viewModel);
    }
}