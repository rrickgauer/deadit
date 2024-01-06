using Deadit.Lib.Domain.Response;
using Deadit.Lib.Domain.TableView;

namespace Deadit.Lib.Service.Contracts;

public interface IBannedCommunityNameService
{
    public Task<ServiceDataResponse<bool>> IsBannedCommunityNameAsync(string communityName);
    public Task<ServiceDataResponse<IEnumerable<ViewBannedCommunityName>>> GetBannedCommunityNamesAsync();
}
