using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Errors;
using Deadit.Lib.Domain.Response;
using Deadit.Lib.Domain.TableView;
using Deadit.Lib.Domain.ViewModel;
using Deadit.Lib.Service.Contracts;

namespace Deadit.Lib.Service.Implementations;

[AutoInject(AutoInjectionType.Scoped, InjectionProject.WebGui, InterfaceType = typeof(IViewModelService))]
public class ViewModelService : IViewModelService
{
    private readonly ICommunityService _communityService;
    private readonly ICommunityMemberService _memberService;
    private readonly IAuthService _authService;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="communityService"></param>
    /// <param name="memberService"></param>
    /// <param name="authService"></param>
    public ViewModelService(ICommunityService communityService, ICommunityMemberService memberService, IAuthService authService)
    {
        _communityService = communityService;
        _memberService = memberService;
        _authService = authService;
    }

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

