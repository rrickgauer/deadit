using Deadit.Lib.Domain.Response;
using Deadit.Lib.Domain.ViewModel;

namespace Deadit.Lib.Service.Contracts;

public interface IViewModelService
{
    public Task<ServiceDataResponse<CommunityPageViewModel>> GetCommunityPageViewModelAsync(string communityName, uint? userId);
    public Task<ServiceDataResponse<JoinedCommunitiesPageViewModel>> GetJoinedCommunitiesPageViewModelAsync(uint userId);
}

