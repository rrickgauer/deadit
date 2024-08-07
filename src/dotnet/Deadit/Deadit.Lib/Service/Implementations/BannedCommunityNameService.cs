﻿using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Errors;
using Deadit.Lib.Domain.Response;
using Deadit.Lib.Domain.TableView;
using Deadit.Lib.Repository.Contracts;
using Deadit.Lib.Service.Contracts;

namespace Deadit.Lib.Service.Implementations;

[AutoInject<IBannedCommunityNameService>(AutoInjectionType.Scoped, InjectionProject.Always)]
public class BannedCommunityNameService(ITableMapperService tableMapperService, IBannedCommunityNameRepository bannedCommunityNameRepository) : IBannedCommunityNameService
{
    private readonly ITableMapperService _tableMapperService = tableMapperService;
    private readonly IBannedCommunityNameRepository _bannedCommunityNameRepository = bannedCommunityNameRepository;

    public async Task<ServiceDataResponse<bool>> IsBannedCommunityNameAsync(string communityName)
    {
        try
        {

            var getBannedNames = await GetBannedCommunityNamesAsync();

            if (!getBannedNames.Successful)
            {
                return new(getBannedNames);
            }

            var bannedNames = getBannedNames.Data?.Select(n => n.Name!.ToLower()).ToList() ?? new();

            bool isBanned = bannedNames.Contains(communityName.ToLower());
            
            return isBanned;
        }
        catch(RepositoryException ex)
        {
            return ex;
        }

    }

    public async Task<ServiceDataResponse<List<ViewBannedCommunityName>>> GetBannedCommunityNamesAsync()
    {
        try
        {
            var datatable = await _bannedCommunityNameRepository.SelectAllBannedCommunityNamesAsync();
            var models = _tableMapperService.ToModels<ViewBannedCommunityName>(datatable);

            return new()
            {
                Data = models,
            };
        }
        catch(RepositoryException ex) 
        {
            return ex;
        }

    }
}
