using Deadit.Lib.Domain.Response;
using Deadit.Lib.Domain.TableView;
using static Deadit.Lib.Domain.Model.Votes;

namespace Deadit.Lib.Service.Contracts;

public interface ICommentVotesService
{
    public Task<ServiceResponse> SaveVoteAsync(VoteComment vote);
    public Task<ServiceDataResponse<List<ViewVoteComment>>> GetVoteCommentsAsync(Guid commentId);
    public Task<ServiceDataResponse<ViewVoteComment>> GetVoteAsync(Guid commentId, uint userId);
    public Task<ServiceDataResponse<List<ViewVoteComment>>> GetUserCommentVotesInPost(Guid postId, uint userId);
}
