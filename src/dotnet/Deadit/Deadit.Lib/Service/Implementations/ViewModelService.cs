using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Errors;
using Deadit.Lib.Domain.Response;
using Deadit.Lib.Domain.TableView;
using Deadit.Lib.Domain.ViewModel;
using Deadit.Lib.Service.Contracts;

namespace Deadit.Lib.Service.Implementations;


[AutoInject<IViewModelService>(AutoInjectionType.Scoped, InjectionProject.WebGui)]
public class ViewModelService(ICommunityService communityService, ICommunityMemberService memberService, IAuthService authService) : IViewModelService
{
    private readonly ICommunityService _communityService = communityService;
    private readonly ICommunityMemberService _memberService = memberService;
    private readonly IAuthService _authService = authService;

    /// <summary>
    /// Get the view model for the CommunityPage
    /// </summary>
    /// <param name="communityName"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundHttpResponseException"></exception>
    public async Task<ServiceDataResponse<CommunityPageViewModel>> GetCommunityPageViewModelAsync(string communityName, uint? userId)
    {
        ViewCommunity community = (await _communityService.GetCommunityAsync(communityName)).Data ?? throw new NotFoundHttpResponseException();
        bool isMember = await IsUserCommunityMemberAsync(community.CommunityId, userId);

        CommunityPageViewModel viewModel = new()
        {
            Community = community,
            IsMember = isMember,
            IsLoggedIn = _authService.IsClientLoggedIn(),
        };

        return new(viewModel);
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

