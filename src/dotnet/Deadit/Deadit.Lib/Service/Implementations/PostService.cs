using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Errors;
using Deadit.Lib.Domain.Model;
using Deadit.Lib.Domain.Response;
using Deadit.Lib.Domain.TableView;
using Deadit.Lib.Repository.Contracts;
using Deadit.Lib.Service.Contracts;

namespace Deadit.Lib.Service.Implementations;

[AutoInject<IPostService>(AutoInjectionType.Scoped, InjectionProject.Always)]
public class PostService(ITableMapperService tableMapperService, IPostRepository postRepository) : IPostService
{
    private readonly ITableMapperService _tableMapperService = tableMapperService;
    private readonly IPostRepository _postRepository = postRepository;

    public async Task<ServiceDataResponse<List<ViewPost>>> GetAllBasicPostsAsync(string communityName)
    {
        try
        {
            var repoResponse = await _postRepository.SelectAllCommunityAsync(communityName);

            var models = _tableMapperService.ToModels<ViewPost>(repoResponse);

            return new(models);
        }
        catch (RepositoryException ex)
        {
            return ex;
        }


    }

    public async Task<ServiceDataResponse<List<ViewPostText>>> GetAllTextPostsAsync(string communityName)
    {
        try
        {
            var repoResponse = await _postRepository.SelectAllCommunityTextAsync(communityName);

            var models = _tableMapperService.ToModels<ViewPostText>(repoResponse);

            return new(models);
        }
        catch (RepositoryException ex)
        {
            return ex;
        }

    }

    public async Task<ServiceDataResponse<List<ViewPostLink>>> GetAllLinkPostsAsync(string communityName)
    {
        try
        {
            var repoResponse = await _postRepository.SelectAllCommunityLinkAsync(communityName);

            var models = _tableMapperService.ToModels<ViewPostLink>(repoResponse);

            return new(models);
        }
        catch (RepositoryException ex)
        {
            return ex;
        }
    }

    public async Task<ServiceDataResponse<ViewPost>> GetPostAsync(Guid postId)
    {
        try
        {
            ServiceDataResponse<ViewPost> result = new();

            var row = await _postRepository.SelectPostAsync(postId);

            if (row != null)
            {
                result.Data = _tableMapperService.ToModel<ViewPost>(row);
            }

            return result;
        }
        catch (RepositoryException ex)
        {
            return ex;
        }
    }

    public async Task<ServiceDataResponse<ViewPostText>> GetTextPostAsync(Guid postId)
    {
        try
        {
            var row = await _postRepository.SelectTextAsync(postId);

            if (row != null)
            {
                var model = _tableMapperService.ToModel<ViewPostText>(row);
                return new(model);
            }

            return new();
        }
        catch(RepositoryException ex)
        {
            return ex;
        }
    }

    public async Task<ServiceDataResponse<ViewPostLink>> GetLinkPostAsync(Guid postId)
    {
        try
        {
            var row = await _postRepository.SelectLinkAsync(postId);

            if (row != null)
            {
                var model = _tableMapperService.ToModel<ViewPostLink>(row);
                return new(model);
            }

            return new();
        }
        catch (RepositoryException ex)
        {
            return ex;
        }
    }



    public async Task<ServiceDataResponse<ViewPostText>> SavePostTextAsync(PostText post)
    {
        // make sure the post has an id value
        if (post.Id is not Guid postId)
        {
            throw new Exception($"{nameof(PostText.Id)} is null");
        }

        // save the post to the database
        try
        {
            var repoResult = await _postRepository.InsertPostAsync(post);
        }
        catch(RepositoryException ex)
        {
            return ex;
        }

        return await GetTextPostAsync(postId);
    }

    public async Task<ServiceDataResponse<ViewPostLink>> SavePostLinkAsync(PostLink post)
    {
        // make sure the post has an id value
        if (post.Id is not Guid postId)
        {
            throw new Exception($"{nameof(PostText.Id)} is null");
        }

        // save the post to the database
        try
        {
            var repoResult = await _postRepository.InsertPostAsync(post);
        }
        catch (RepositoryException ex)
        {
            return ex;
        }

        return await GetLinkPostAsync(postId);
    }
}
