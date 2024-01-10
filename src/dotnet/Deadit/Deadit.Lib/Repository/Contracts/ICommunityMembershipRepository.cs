using System.Data;

namespace Deadit.Lib.Repository.Contracts;

public interface ICommunityMembershipRepository
{
    public Task<DataTable> SelectUserJoinedCommunitiesAsync(uint userId);
    public Task<DataRow?> SelectByUserIdAndCommunityIdAsync(uint userId, uint communityId);
    public Task<int> DeleteMembershipAsync(uint userId, string communityName);
}
