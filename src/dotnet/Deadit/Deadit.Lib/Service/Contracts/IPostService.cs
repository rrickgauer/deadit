using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Model;
using Deadit.Lib.Domain.Response;
using Deadit.Lib.Domain.TableView;

namespace Deadit.Lib.Service.Contracts;

public interface IPostService
{
    public Task<ServiceDataResponse<List<ViewPost>>> GetAllBasicPostsAsync(string communityName);
    public Task<ServiceDataResponse<List<ViewPostText>>> GetAllTextPostsAsync(string communityName);
    public Task<ServiceDataResponse<List<ViewPostLink>>> GetAllLinkPostsAsync(string communityName);

    public Task<ServiceDataResponse<List<ViewPost>>> GetTopCommunityPostsAsync(string communityName, TopPostSort sortedBy);

    public Task<ServiceDataResponse<ViewPost>> GetPostAsync(Guid postId);
    public Task<ServiceDataResponse<ViewPostText>> GetTextPostAsync(Guid postId);
    public Task<ServiceDataResponse<ViewPostLink>> GetLinkPostAsync(Guid postId);

    public Task<ServiceDataResponse<ViewPostText>> SavePostTextAsync(PostText post);
    public Task<ServiceDataResponse<ViewPostLink>> SavePostLinkAsync(PostLink post);

}


