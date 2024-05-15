using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Errors;
using Deadit.Lib.Domain.Response;
using Deadit.Lib.Domain.TableView;
using Deadit.Lib.Repository.Contracts;
using Deadit.Lib.Service.Contracts;
using static Deadit.Lib.Domain.Model.Votes;

namespace Deadit.Lib.Service.Implementations;


[AutoInject<IPostVotesService>(AutoInjectionType.Scoped, InjectionProject.Always)]
public class PostVotesService(IPostVotesRepository repo, ITableMapperService mapperService) : IPostVotesService
{
    private readonly IPostVotesRepository _repo = repo;
    private readonly ITableMapperService _mapperService = mapperService;

    public async Task<ServiceDataResponse<ViewVotePost>> SaveVoteAsync(VotePost vote)
    {
        var postId = NotFoundHttpResponseException.ThrowIfNot<Guid>(vote.ItemId);
        var userId = NotFoundHttpResponseException.ThrowIfNot<uint>(vote.UserId);

        try
        {
            await _repo.SaveVoteAsync(vote);
        }
        catch(RepositoryException ex)
        {
            return ex;
        }

        try
        {
            return await GetVoteAsync(postId, userId);
        }
        catch(ServiceResponseException ex)
        {
            return new(ex);
        }
        
    }



    public async Task<ServiceDataResponse<List<ViewVotePost>>> GetPostVotesAsync(Guid postId)
    {
        try
        {

            var table = await _repo.SelectPostVotesAsync(postId);

            var models = _mapperService.ToModels<ViewVotePost>(table);

            return new(models);
        }
        catch (RepositoryException ex)
        {
            return ex;
        }
    }

    public async Task<ServiceDataResponse<ViewVotePost>> GetVoteAsync(Guid postId, uint userId)
    {
        try
        {
            ServiceDataResponse<ViewVotePost> result = new();

            var row = await _repo.SelectVoteAsync(postId, userId);

            if (row != null)
            {
                result.Data = _mapperService.ToModel<ViewVotePost>(row);    
            }

            return result;
        }
        catch (RepositoryException ex)
        {
            return ex;
        }
    }



}
