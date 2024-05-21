using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Errors;
using Deadit.Lib.Domain.Response;
using Deadit.Lib.Domain.TableView;
using Deadit.Lib.Repository.Contracts;
using Deadit.Lib.Service.Contracts;

namespace Deadit.Lib.Service.Implementations;

[AutoInject<IApiAccessTokenService>(AutoInjectionType.Scoped, InjectionProject.Always)]
public class ApiAccessTokenService(IApiAccessTokenRepository repo, ITableMapperService tableMapperService) : IApiAccessTokenService
{
    private readonly IApiAccessTokenRepository _repo = repo;
    private readonly ITableMapperService _tableMapperService = tableMapperService;

    public async Task<ServiceDataResponse<ViewApiAccessToken>> GetTokenAsync(Guid token)
    {
        try
        {
            ServiceDataResponse<ViewApiAccessToken> result = new();

            var row = await _repo.SelectTokenAsync(token);

            if (row != null) 
            { 
                result.Data = _tableMapperService.ToModel<ViewApiAccessToken>(row);
            }

            return result;
        }
        catch(RepositoryException ex)
        {
            return ex;
        }

    }
}