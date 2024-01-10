using Deadit.Lib.Domain.Forms;
using Deadit.Lib.Domain.Response;
using Deadit.Lib.Domain.TableView;
using Deadit.Lib.Domain.ViewModel;

namespace Deadit.Lib.Service.Contracts;

public interface ICommunityService
{
    public Task<ServiceDataResponse<CommunityPageViewModel>> GetCommunityPageViewModelAsync(string communityName, uint? userId);
    public Task<ServiceDataResponse<ViewCommunity>> CreateCommunityAsync(CreateCommunityRequestForm form, uint userId);
    public Task<ServiceDataResponse<ViewCommunity>> GetCommunityAsync(string communityName);
    public Task<ServiceDataResponse<ViewCommunity>> GetCommunityAsync(uint communityId);
}
