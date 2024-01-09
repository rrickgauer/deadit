using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Response;
using Deadit.Lib.Domain.TableView;
using Deadit.Lib.Repository.Contracts;
using Deadit.Lib.Service.Contracts;

namespace Deadit.Lib.Service.Implementations;

[AutoInject(AutoInjectionType.Scoped, InjectionProject.Always, InterfaceType = typeof(ICommunityMemberService))]
public class CommunityMemberService : ICommunityMemberService
{
    private readonly ICommunityMembershipRepository _repo;
    private readonly ITableMapperService _tableMapperService;

    public CommunityMemberService(ICommunityMembershipRepository repo, ITableMapperService tableMapperService)
    {
        _repo = repo;
        _tableMapperService = tableMapperService;
    }

    public async Task<ServiceDataResponse<IEnumerable<ViewCommunityMembership>>> GetJoinedCommunitiesAsync(uint userId)
    {
        var datatable = await _repo.SelectUserJoinedCommunitiesAsync(userId);
        var models = _tableMapperService.ToModels<ViewCommunityMembership>(datatable);
        return new(models);
        
    }
}
