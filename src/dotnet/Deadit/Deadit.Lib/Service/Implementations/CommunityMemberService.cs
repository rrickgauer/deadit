using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Response;
using Deadit.Lib.Domain.TableView;
using Deadit.Lib.Repository.Contracts;
using Deadit.Lib.Service.Contracts;
using System.Security.Cryptography.X509Certificates;

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


    public async Task<ServiceDataResponse<bool>> IsMemberAsync(uint userId, uint communityId)
    {
        var getMembershipResponse = (await GetMembershipAsync(userId, communityId)).Data;

        if (getMembershipResponse == null)
        {
            return new(false);
        }

        return new(true);
    }

    public async Task<ServiceDataResponse<ViewCommunityMembership>> GetMembershipAsync(uint userId, uint communityId)
    {
        ServiceDataResponse<ViewCommunityMembership> response = new();

        var datarow = await _repo.SelectByUserIdAndCommunityIdAsync(userId, communityId);
        
        if (datarow != null)
        {
            response.Data = _tableMapperService.ToModel<ViewCommunityMembership>(datarow);
        }

        return response;
    }

    public async Task<ServiceDataResponse<int>> LeaveCommunityAsync(uint userId, string communityName)
    {
        var numRecords = await _repo.DeleteMembershipAsync(userId, communityName);

        return new(numRecords);
    }
}
