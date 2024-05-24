using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Forms;
using Deadit.Lib.Domain.Model;
using Deadit.Lib.Domain.Paging;
using Deadit.Lib.Domain.Response;
using Deadit.Lib.Domain.TableView;

namespace Deadit.Lib.Service.Contracts;

public interface IPostService
{

    public Task<ServiceDataResponse<List<ViewPost>>> GetUserNewHomeFeedAsycn(uint clientId, PaginationPosts pagination);
    public Task<ServiceDataResponse<List<ViewPost>>> GetUserTopHomeFeedAsync(uint clientId, PaginationPosts pagination, TopPostSort sort);

    public Task<ServiceDataResponse<List<ViewPost>>> GetNewestCommunityPostsAsync(string communityName, PaginationPosts pagination);
    public Task<ServiceDataResponse<List<ViewPost>>> GetNewestCommunityPostsAsync(string communityName);

    public Task<ServiceDataResponse<List<ViewPost>>> GetTopCommunityPostsAsync(string communityName, TopPostSort sortedBy, PaginationPosts pagination);
    public Task<ServiceDataResponse<List<ViewPost>>> GetTopCommunityPostsAsync(string communityName, TopPostSort sortedBy);

    public Task<ServiceDataResponse<ViewPost>> GetPostAsync(Guid postId);
    public Task<ServiceDataResponse<ViewPostText>> GetTextPostAsync(Guid postId);
    public Task<ServiceDataResponse<ViewPostLink>> GetLinkPostAsync(Guid postId);

    public Task<ServiceDataResponse<ViewPostText>> CreatePostTextAsync(PostText post);
    public Task<ServiceDataResponse<ViewPostLink>> CreatePostLinkAsync(PostLink post);

    public Task<ServiceDataResponse<ViewPostText>> SavePostTextAsync(PostText post);

    public Task<ServiceResponse> AuthorDeletePostAsync(Guid postId);

    public Task<ServiceResponse> LockPostCommentsAsync(Guid postId);
    public Task<ServiceResponse> UnlockPostCommentsAsync(Guid postId);

    public Task<ServiceResponse> ModeratePostAsync(Guid postId, ModeratePostForm form);
}


