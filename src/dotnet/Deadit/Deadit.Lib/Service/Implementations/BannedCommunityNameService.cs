using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Response;
using Deadit.Lib.Domain.TableView;
using Deadit.Lib.Repository.Contracts;
using Deadit.Lib.Service.Contracts;

namespace Deadit.Lib.Service.Implementations;

[AutoInject(AutoInjectionType.Scoped, InjectionProject.Always, InterfaceType = typeof(IBannedCommunityNameService))]
public class BannedCommunityNameService : IBannedCommunityNameService
{
    private readonly ITableMapperService _tableMapperService;
    private readonly IBannedCommunityNameRepository _bannedCommunityNameRepository;

    public BannedCommunityNameService(ITableMapperService tableMapperService, IBannedCommunityNameRepository bannedCommunityNameRepository)
    {
        _tableMapperService = tableMapperService;
        _bannedCommunityNameRepository = bannedCommunityNameRepository;
    }


    public async Task<ServiceDataResponse<bool>> IsBannedCommunityNameAsync(string communityName)
    {
        var bannedCommunityNames = (await GetBannedCommunityNamesAsync())?.Data?.Select(n => n.Name?.ToLower()).ToList();

        bool isBanned = bannedCommunityNames?.Contains(communityName.ToLower()) ?? false;

        return new()
        {
            Data = isBanned,
        };
    }

    public async Task<ServiceDataResponse<IEnumerable<ViewBannedCommunityName>>> GetBannedCommunityNamesAsync()
    {
        var datatable = await _bannedCommunityNameRepository.SelectAllBannedCommunityNamesAsync();
        var models = _tableMapperService.ToModels<ViewBannedCommunityName>(datatable);

        return new()
        {
            Data = models,
        };
    }
}
