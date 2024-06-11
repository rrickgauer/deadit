using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Errors;
using Deadit.Lib.Domain.Parms;
using Deadit.Lib.Domain.Response;
using Deadit.Lib.Domain.TableView;
using Deadit.Lib.Domain.ViewModel;
using Deadit.Lib.Service.Contracts;


using ViewModel = Deadit.Lib.Domain.ViewModel.CommunitySettingsLayoutModel<Deadit.Lib.Domain.ViewModel.MembersCommunitySettingsPageModel>;

namespace Deadit.Lib.Service.ViewModels;

[AutoInject(AutoInjectionType.Scoped, InjectionProject.WebGui)]
public class MembersCommunitySettingsVMService : IVMService<MembersCommunitySettingsPageParms, ViewModel>
{
    private readonly ICommunityService _communityService;
    private readonly ICommunityMemberService _communityMemberService;

    public MembersCommunitySettingsVMService(ICommunityService communityService, ICommunityMemberService communityMemberService)
    {
        _communityService = communityService;
        _communityMemberService = communityMemberService;
    }

    public async Task<ServiceDataResponse<ViewModel>> GetViewModelAsync(MembersCommunitySettingsPageParms args)
    {
        try
        {
            var community = await GetCommunityAsync(args.CommunityName);
            var memberships = await GetMembershipsAsync(args);

            MembersCommunitySettingsPageModel pageModel = new()
            {
                Memberships = memberships,
                Sorting = args.CommunityMembersSorting,
                Pagination = args.Pagination,
            };

            ViewModel viewModel = new()
            {
                PageTitle = "Member Settings",
                PageModel = pageModel,
                Community = community,
                ActivePage = ActiveCommunitySettingsPage.Members,
            };


            return new(viewModel);
        }
        catch(ServiceResponseException ex)
        {
            return new(ex.Response);
        }
    }


    private async Task<ViewCommunity> GetCommunityAsync(string communityName)
    {
        var getCommunity = await _communityService.GetCommunityAsync(communityName);

        getCommunity.ThrowIfError();

        if (getCommunity.Data is not ViewCommunity community)
        {
            throw new NotFoundHttpResponseException();
        }

        return community;
    }


    private async Task<List<ViewCommunityMembership>> GetMembershipsAsync(MembersCommunitySettingsPageParms args)
    {
        var getMemberships = await _communityMemberService.GetCommunityMembersAsync(args.CommunityName, args.CommunityMembersSorting, args.Pagination);

        getMemberships.ThrowIfError();

        return getMemberships.Data ?? new();
    }




}


