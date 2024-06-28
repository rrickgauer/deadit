using Deadit.Lib.Domain.Model;
using System.Data;

namespace Deadit.Lib.Repository.Contracts;

public interface IFlairPostRepository
{
    public Task<DataTable> SelectCommunityPostFlairsAsync(uint communityId);
    public Task<DataTable> SelectCommunityPostFlairsAsync(string communityName);
    public Task<DataRow?> SelectPostFlairAsync(uint flairId);
    public Task<int> UpdateFlairAsync(FlairPost flair);
    public Task<uint?> InsertFlairAsync(FlairPost flair);
    public Task<int> DeleteFlairAsync(uint flairId);
}
