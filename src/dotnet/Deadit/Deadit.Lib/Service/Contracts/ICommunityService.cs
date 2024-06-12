using Deadit.Lib.Domain.Forms;
using Deadit.Lib.Domain.Model;
using Deadit.Lib.Domain.Response;
using Deadit.Lib.Domain.TableView;

namespace Deadit.Lib.Service.Contracts;

public interface ICommunityService
{
    public Task<ServiceDataResponse<ViewCommunity>> CreateCommunityAsync(CreateCommunityRequestForm form, uint userId);
    public Task<ServiceDataResponse<ViewCommunity>> GetCommunityAsync(string communityName);
    public Task<ServiceDataResponse<ViewCommunity>> GetCommunityAsync(uint communityId);
    public Task<ServiceDataResponse<List<ViewCommunity>>> GetCreatedCommunitiesAsync(uint userId);
    public Task<ServiceDataResponse<ViewCommunity>> SaveCommunityAsync(Community community);
}
