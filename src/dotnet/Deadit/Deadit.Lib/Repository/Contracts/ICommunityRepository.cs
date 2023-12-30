using Deadit.Lib.Domain.Forms;
using System.Data;

namespace Deadit.Lib.Repository.Contracts;

public interface ICommunityRepository
{
    public Task<DataRow?> SelectCommunityAsync(string communityName);
    public Task<DataRow?> SelectCommunityAsync(uint communityId);
    public Task<uint?> InsertCommunityAsync(CreateCommunityRequestForm createCommunity, uint userId);
}
