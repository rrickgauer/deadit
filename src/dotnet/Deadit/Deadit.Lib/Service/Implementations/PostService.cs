using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Errors;
using Deadit.Lib.Domain.Forms;
using Deadit.Lib.Domain.Model;
using Deadit.Lib.Domain.Paging;
using Deadit.Lib.Domain.Response;
using Deadit.Lib.Domain.TableView;
using Deadit.Lib.Repository.Contracts;
using Deadit.Lib.Service.Contracts;
using System.Runtime.InteropServices;

namespace Deadit.Lib.Service.Implementations;

[AutoInject<IPostService>(AutoInjectionType.Scoped, InjectionProject.Always)]
public class PostService(ITableMapperService tableMapperService, IPostRepository postRepository) : IPostService
{
    #region - Private Members -
    private readonly ITableMapperService _tableMapperService = tableMapperService;
    private readonly IPostRepository _postRepository = postRepository;
    #endregion

    #region - Home Feed -

    public async Task<ServiceDataResponse<List<ViewPost>>> GetUserNewHomeFeedAsycn(uint clientId, PaginationPosts pagination)
    {
        try
        {
            var table = await _postRepository.SelectUserNewHomePostsAsnc(clientId, pagination);

            var posts = _tableMapperService.ToModels<ViewPost>(table);

            return posts;
        }
        catch(RepositoryException ex)
        {
            return ex;
        }
    }

    public async Task<ServiceDataResponse<List<ViewPost>>> GetUserTopHomeFeedAsync(uint clientId, PaginationPosts pagination, TopPostSort sort)
    {
        try
        {
            var createdAfter = sort.GetStartingDate();
            var table = await _postRepository.SelectUserTopHomePostsAsnc(clientId, pagination, createdAfter);
            var posts = _tableMapperService.ToModels<ViewPost>(table);

            return posts;
        }
        catch (RepositoryException ex)
        {
            return ex;
        }
    }


    #endregion

    #region - Newest Community Posts -

    public async Task<ServiceDataResponse<List<ViewPost>>> GetNewestCommunityPostsAsync(string communityName, PaginationPosts pagination, uint flairId)
    {
        try
        {
            var table = await _postRepository.SelectNewestCommunityPostsAsync(communityName, pagination, flairId);

            var models = _tableMapperService.ToModels<ViewPost>(table);

            return models;
        }
        catch (RepositoryException ex)
        {
            return ex;
        }
    }

    public async Task<ServiceDataResponse<List<ViewPost>>> GetNewestCommunityPostsAsync(string communityName, PaginationPosts pagination)
    {
        try
        {
            var table = await _postRepository.SelectNewestCommunityPostsAsync(communityName, pagination);

            var models = _tableMapperService.ToModels<ViewPost>(table);

            return models;
        }
        catch (RepositoryException ex)
        {
            return ex;
        }
    }

    public async Task<ServiceDataResponse<List<ViewPost>>> GetNewestCommunityPostsAsync(string communityName)
    {
        try
        {
            var repoResponse = await _postRepository.SelectNewestCommunityPostsAsync(communityName);

            var models = _tableMapperService.ToModels<ViewPost>(repoResponse);

            return new(models);
        }
        catch (RepositoryException ex)
        {
            return ex;
        }
    }

    #endregion

    #region - Top Community Posts -

    public async Task<ServiceDataResponse<List<ViewPost>>> GetTopCommunityPostsAsync(string communityName, TopPostSort sortedBy, PaginationPosts pagination, uint flairId)
    {
        try
        {
            var table = await _postRepository.SelectTopCommunityPostsAsync(communityName, sortedBy.GetStartingDate(), pagination, flairId);

            var models = _tableMapperService.ToModels<ViewPost>(table);

            return models;
        }
        catch (RepositoryException ex)
        {
            return ex;
        }
    }

    public async Task<ServiceDataResponse<List<ViewPost>>> GetTopCommunityPostsAsync(string communityName, TopPostSort sortedBy, PaginationPosts pagination)
    {
        try
        {
            var table = await _postRepository.SelectTopCommunityPostsAsync(communityName, sortedBy.GetStartingDate(), pagination);

            var models = _tableMapperService.ToModels<ViewPost>(table);

            return models;
        }
        catch (RepositoryException ex)
        {
            return ex;
        }
    }


    public async Task<ServiceDataResponse<List<ViewPost>>> GetTopCommunityPostsAsync(string communityName, TopPostSort sortedBy)
    {
        try
        {
            var table = await _postRepository.SelectTopCommunityPostsAsync(communityName, sortedBy.GetStartingDate());

            var models = _tableMapperService.ToModels<ViewPost>(table);

            return new(models);
        }
        catch (RepositoryException ex)
        {
            return ex;
        }

    }


    #endregion


    #region - Get Single Post -


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
            var row = await _postRepository.SelectPostTextAsync(postId);

            if (row != null)
            {
                var model = _tableMapperService.ToModel<ViewPostText>(row);
                return new(model);
            }

            return new();
        }
        catch (RepositoryException ex)
        {
            return ex;
        }
    }

    public async Task<ServiceDataResponse<ViewPostLink>> GetLinkPostAsync(Guid postId)
    {
        try
        {
            var row = await _postRepository.SelectPostLinkAsync(postId);

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


    #endregion

    #region - Save -

    public async Task<ServiceDataResponse<ViewPostText>> CreatePostTextAsync(PostText post)
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


    public async Task<ServiceDataResponse<ViewPostLink>> CreatePostLinkAsync(PostLink post)
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
            var repoResult = await _postRepository.UpdatePostAsync(post);
        }
        catch (RepositoryException ex)
        {
            return ex;
        }

        return await GetTextPostAsync(postId);
    }

    #endregion



    #region - Delete -

    public async Task<ServiceResponse> AuthorDeletePostAsync(Guid postId)
    {
        try
        {

            var result = await _postRepository.MarkPostDeletedAsync(postId);

            return new();
        }
        catch(RepositoryException ex)
        {
            return ex;
        }
    }


    #endregion


    #region - Lock/Unlock Post -

    
    
    public async Task<ServiceResponse> LockPostCommentsAsync(Guid postId)
    {
        try
        {
            await _postRepository.UpdatePostLockedAsync(postId, true);

            return new();
        }
        catch(RepositoryException ex)
        {
            return ex;
        }
    }
    
    
    public async Task<ServiceResponse> UnlockPostCommentsAsync(Guid postId)
    {
        try
        {
            await _postRepository.UpdatePostLockedAsync(postId, false);

            return new();
        }
        catch (RepositoryException ex)
        {
            return ex;
        }
    }

    #endregion


    public async Task<ServiceResponse> ModeratePostAsync(Guid postId, ModeratePostForm form)
    {
        try
        {
            await _postRepository.UpdatePostModerationFieldsAsync(postId, form);

            return new();

        }
        catch(RepositoryException ex)
        {
            return ex;
        }
    }
}
