using Deadit.Lib.Domain.Response;
using Deadit.Lib.Domain.TableView;
using static Deadit.Lib.Domain.Model.Votes;

namespace Deadit.Lib.Service.Contracts;

public interface IPostVotesService
{
    public Task<ServiceDataResponse<ViewVotePost>> SaveVoteAsync(VotePost vote);
    public Task<ServiceDataResponse<List<ViewVotePost>>> GetPostVotesAsync(Guid postId);
    public Task<ServiceDataResponse<ViewVotePost>> GetVoteAsync(Guid postId, uint userId);
    public Task<ServiceDataResponse<List<ViewVotePost>>> GetUserPostVotesInCommunity(uint userId, string communityName);


    public Task<ServiceDataResponse<List<ViewVotePost>>> GetUserPostVotesAsync(uint userId, IEnumerable<Guid> postIds);


}
