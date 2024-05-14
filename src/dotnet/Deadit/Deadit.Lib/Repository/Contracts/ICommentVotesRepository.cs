using Deadit.Lib.Domain.Forms;
using System.Data;
using static Deadit.Lib.Domain.Model.Votes;

namespace Deadit.Lib.Repository.Contracts;

public interface ICommentVotesRepository
{
    public Task<int> SaveVoteAsync(VoteComment vote);

    public Task<DataTable> SelectCommentVotesAsync(Guid commentId);
    public Task<DataTable> SelectUserVotesInPost(Guid postId, uint userId);
    public Task<DataRow?> SelectVoteAsync(Guid commentId, uint userId);
}
