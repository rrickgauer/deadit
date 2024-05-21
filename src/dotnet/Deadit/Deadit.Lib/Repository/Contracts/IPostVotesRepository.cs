using System.Data;
using static Deadit.Lib.Domain.Model.Votes;

namespace Deadit.Lib.Repository.Contracts;

public interface IPostVotesRepository
{
    public Task<int> SaveVoteAsync(VotePost vote);

    public Task<DataTable> SelectPostVotesAsync(Guid postId);
    public Task<DataRow?> SelectVoteAsync(Guid postId, uint userId);
    public Task<DataTable> SelectUserPostVotesInCommunityAsync(uint userId, string communityName);
    public Task<DataTable> SelectUserPostVotesAsync(uint userId, IEnumerable<Guid> postIds);
}
