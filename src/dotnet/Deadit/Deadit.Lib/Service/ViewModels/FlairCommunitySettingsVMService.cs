namespace Deadit.Lib.Service.ViewModels;

using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Errors;
using Deadit.Lib.Domain.Parms;
using Deadit.Lib.Domain.Response;
using Deadit.Lib.Domain.TableView;
using Deadit.Lib.Domain.ViewModel;
using Deadit.Lib.Service.Contracts;
using System.Threading.Tasks;
using ViewModel = Domain.ViewModel.CommunitySettingsLayoutModel<Domain.ViewModel.FlairCommunitySettingsPageModel>;

[AutoInject(AutoInjectionType.Scoped, InjectionProject.WebGui)]
public class FlairCommunitySettingsVMService(ICommunityService communityService, IFlairPostService flairService) : IVMService<FlairCommunitySettingsPageParms, ViewModel>
{
    private readonly ICommunityService _communityService = communityService;
    private readonly IFlairPostService _flairService = flairService;

    public async Task<ServiceDataResponse<ViewModel>> GetViewModelAsync(FlairCommunitySettingsPageParms args)
    {
        try
        {
            var community = await GetCommunityAsync(args);

            var flairs = await GetFlairsAsync(community.CommunityId!.Value);

            FlairCommunitySettingsPageModel pageModel = new()
            {
                Flairs = flairs,
            };

            ViewModel vm = new()
            {
                ActivePage = ActiveCommunitySettingsPage.Flair,
                Community = community,
                PageModel = pageModel,
                PageTitle = "Community Flair",
            };

            return vm;

        }
        catch (ServiceResponseException ex)
        {
            return ex;
        }
    }


    private async Task<ViewCommunity> GetCommunityAsync(FlairCommunitySettingsPageParms args)
    {
        var getCommunity = await _communityService.GetCommunityAsync(args.CommunityName);

        getCommunity.ThrowIfError();

        if (getCommunity.Data is not ViewCommunity community)
        {
            throw new NotFoundHttpResponseException();
        }

        return community;
    }


    private async Task<List<ViewFlairPost>> GetFlairsAsync(uint communityId)
    {
        var getFlairs = await _flairService.GetFlairPostsAsync(communityId);

        var flairs = getFlairs.Data ?? new();

        return flairs;
    }


}
