using System.Data;

namespace Deadit.Lib.Repository.Contracts;

public interface IApiAccessTokenRepository
{
    public Task<DataRow?> SelectTokenAsync(Guid token);
}
