using Deadit.Lib.Domain.Response;
using Deadit.Lib.Domain.TableView;

namespace Deadit.Lib.Service.Contracts;

public interface IApiAccessTokenService
{
    public Task<ServiceDataResponse<ViewApiAccessToken>> GetTokenAsync(Guid token);
}
