using System.Data;

namespace Deadit.Lib.Repository.Contracts;

public interface IBannedCommunityNameRepository
{
    public Task<DataTable> SelectAllBannedCommunityNamesAsync();
}
