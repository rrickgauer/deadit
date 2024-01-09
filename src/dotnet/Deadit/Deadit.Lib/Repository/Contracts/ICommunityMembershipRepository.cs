using System.Data;

namespace Deadit.Lib.Repository.Contracts;

public interface ICommunityMembershipRepository
{
    public Task<DataTable> SelectUserJoinedCommunitiesAsync(uint userId);
}
