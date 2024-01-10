using Deadit.Lib.Domain.Dto;
using Deadit.Lib.Domain.Response;
using Deadit.Lib.Domain.TableView;

namespace Deadit.Lib.Service.Contracts;

public interface ICommunityMemberService
{
    public Task<ServiceDataResponse<IEnumerable<ViewCommunityMembership>>> GetJoinedCommunitiesAsync(uint userId);
    public Task<ServiceDataResponse<bool>> IsMemberAsync(uint userId, uint communityId);
    public Task<ServiceDataResponse<ViewCommunityMembership>> GetMembershipAsync(uint userId, uint communityId);
    public Task<ServiceDataResponse<ViewCommunityMembership>> GetMembershipAsync(uint userId, string communityName);
    public Task<ServiceDataResponse<int>> LeaveCommunityAsync(uint userId, string communityName);
    public Task<ServiceDataResponse<GetJoinedCommunity>> JoinCommunityAsync(uint userId, string communityName);

}
