using Deadit.Lib.Domain.Response;
using Deadit.Lib.Domain.TableView;

namespace Deadit.Lib.Service.Contracts;

public interface ICommunityMemberService
{
    public Task<ServiceDataResponse<IEnumerable<ViewCommunityMembership>>> GetJoinedCommunitiesAsync(uint userId);
}
