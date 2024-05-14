using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Errors;
using Deadit.Lib.Domain.Forms;
using Deadit.Lib.Domain.Response;
using Deadit.Lib.Domain.TableView;
using Deadit.Lib.Repository.Contracts;
using Deadit.Lib.Service.Contracts;
using static Deadit.Lib.Domain.Model.Votes;

namespace Deadit.Lib.Service.Implementations;

[AutoInject<ICommentVotesService>(AutoInjectionType.Scoped, InjectionProject.Always)]
public class CommentVotesService(ITableMapperService tableMapperService, ICommentVotesRepository repo) : ICommentVotesService
{
    private readonly ICommentVotesRepository _repo = repo;
    private readonly ITableMapperService _tableMapperService = tableMapperService;

    public async Task<ServiceResponse> SaveVoteAsync(VoteComment vote)
    {
        try
        {
            var numrecords = await _repo.SaveVoteAsync(vote);
            return new();
        }
        catch (RepositoryException ex)
        {
            return ex;
        }
    }

    public async Task<ServiceDataResponse<List<ViewVoteComment>>> GetVoteCommentsAsync(Guid commentId)
    {
        try
        {
            var table = await _repo.SelectCommentVotesAsync(commentId);
            var models = _tableMapperService.ToModels<ViewVoteComment>(table);
            return new(models);
        }
        catch (RepositoryException ex)
        {
            return ex;
        }
    }

    public async Task<ServiceDataResponse<ViewVoteComment>> GetVoteAsync(Guid commentId, uint userId)
    {
        try
        {
            ServiceDataResponse<ViewVoteComment> result = new();

            var row = await _repo.SelectVoteAsync(commentId, userId);

            if (row != null)
            {
                result.Data = _tableMapperService.ToModel<ViewVoteComment>(row);
            }

            return result;
        }
        catch (RepositoryException ex)
        {
            return ex;
        }
    }


    public async Task<ServiceDataResponse<List<ViewVoteComment>>> GetUserCommentVotesInPost(Guid postId, uint userId)
    {
        try
        {
            var table = await _repo.SelectUserVotesInPost(postId, userId);
            var models = _tableMapperService.ToModels<ViewVoteComment>(table);
            return new(models);
        }
        catch (RepositoryException ex)
        {
            return ex;
        }
    }


}
