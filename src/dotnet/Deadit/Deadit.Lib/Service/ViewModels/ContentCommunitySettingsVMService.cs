using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Errors;
using Deadit.Lib.Domain.Parms;
using Deadit.Lib.Domain.Response;
using Deadit.Lib.Domain.TableView;
using Deadit.Lib.Domain.ViewModel;
using Deadit.Lib.Service.Contracts;


using ViewModel = Deadit.Lib.Domain.ViewModel.CommunitySettingsLayoutModel<Deadit.Lib.Domain.ViewModel.ContentCommunitySettingsPageModel>;

namespace Deadit.Lib.Service.ViewModels;

[AutoInject(AutoInjectionType.Scoped, InjectionProject.WebGui)]
public class ContentCommunitySettingsVMService(ICommunityService communityService) : IVMService<GeneralCommunitySettingsPageParms, ViewModel>
{
    private readonly ICommunityService _communityService = communityService;

    public async Task<ServiceDataResponse<ViewModel>> GetViewModelAsync(GeneralCommunitySettingsPageParms args)
    {
        var getCommunity = await _communityService.GetCommunityAsync(args.CommunityName);

        if (!getCommunity.Successful)
        {
            return new(getCommunity);
        }

        var community = NotFoundHttpResponseException.ThrowIfNot<ViewCommunity>(getCommunity.Data);

        ContentCommunitySettingsPageModel pageModel = new()
        {
            
        };

        ViewModel viewModel = new()
        {
            PageTitle = "Content Settings",
            PageModel = pageModel,
            Community = community,
            ActivePage = ActiveCommunitySettingsPage.Content,
        };


        return new(viewModel);
    }
}


