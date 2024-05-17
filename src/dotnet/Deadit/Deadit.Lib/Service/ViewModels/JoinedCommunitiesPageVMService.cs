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
public class JoinedCommunitiesPageVMService(ICommunityMemberService memberService) : IVMService<GetJoinedCommunitiesPageParms, JoinedCommunitiesPageViewModel>
{
    private readonly ICommunityMemberService _memberService = memberService;

    public async Task<ServiceDataResponse<JoinedCommunitiesPageViewModel>> GetViewModelAsync(GetJoinedCommunitiesPageParms args)
    {
        if (args.ClientId is not uint userId)
        {
            throw new ForbiddenHttpResponseException();
        }

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
}
