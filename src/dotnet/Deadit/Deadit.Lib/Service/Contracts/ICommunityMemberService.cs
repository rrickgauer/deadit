using Deadit.Lib.Domain.Dto;
using Deadit.Lib.Domain.Other;
using Deadit.Lib.Domain.Paging;
using Deadit.Lib.Domain.Response;
using Deadit.Lib.Domain.TableView;

namespace Deadit.Lib.Service.Contracts;

public interface ICommunityMemberService
{
    public Task<ServiceDataResponse<List<ViewCommunityMembership>>> GetJoinedCommunitiesAsync(uint userId);
    public Task<ServiceDataResponse<bool>> IsMemberAsync(uint userId, uint communityId);
    public Task<ServiceDataResponse<ViewCommunityMembership>> GetMembershipAsync(uint userId, uint communityId);
    public Task<ServiceDataResponse<ViewCommunityMembership>> GetMembershipAsync(uint userId, string communityName);
    public Task<ServiceDataResponse<ViewCommunityMembership>> GetMembershipAsync(string username, string communityName);
    public Task<ServiceDataResponse<int>> LeaveCommunityAsync(uint userId, string communityName);
    public Task<ServiceDataResponse<GetJoinedCommunity>> JoinCommunityAsync(uint userId, string communityName);
    public Task<ServiceResponse> RemoveMemberAsync(string username, string communityName);
    public Task<ServiceDataResponse<List<ViewCommunityMembership>>> GetCommunityMembersAsync(string communityName, CommunityMembersSorting sorting, PaginationCommunityMembers pagination);
}
