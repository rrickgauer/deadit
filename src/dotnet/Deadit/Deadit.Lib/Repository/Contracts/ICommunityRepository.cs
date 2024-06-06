using Deadit.Lib.Domain.Forms;
using Deadit.Lib.Domain.Model;
using System.Data;

namespace Deadit.Lib.Repository.Contracts;

public interface ICommunityRepository
{
    public Task<DataRow?> SelectCommunityAsync(string communityName);
    public Task<DataRow?> SelectCommunityAsync(uint communityId);
    public Task<uint?> InsertCommunityAsync(CreateCommunityRequestForm createCommunity, uint userId);

    public Task<DataTable> SelectCreatedCommunitiesAsync(uint userId);

    public Task<int> UpdateCommunityAsync(Community community);
}
