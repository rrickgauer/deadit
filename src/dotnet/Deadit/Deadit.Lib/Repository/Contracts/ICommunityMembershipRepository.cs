using Deadit.Lib.Domain.Other;
using Deadit.Lib.Domain.Paging;
using System.Data;

namespace Deadit.Lib.Repository.Contracts;

public interface ICommunityMembershipRepository
{
    public Task<DataTable> SelectUserJoinedCommunitiesAsync(uint userId);
    public Task<DataRow?> SelectCommunityMembershipAsync(uint userId, uint communityId);
    public Task<DataRow?> SelectCommunityMembershipAsync(uint userId, string communityName);
    public Task<DataRow?> SelectCommunityMembershipAsync(string username, string communityName);
    public Task<int> DeleteMembershipAsync(uint userId, string communityName);
    public Task<int> InsertMembershipAsync(uint userId, string communityName);

    public Task<DataTable> SelectCommunityMembersAsync(string communityName);
    public Task<DataTable> SelectCommunityMembersAsync(string communityName, CommunityMembersSorting sorting, PaginationCommunityMembers pagination);
}
