using Deadit.Lib.Domain.Model;
using Deadit.Lib.Domain.Paging;
using System.Data;

namespace Deadit.Lib.Repository.Contracts;

public interface IPostRepository
{
    public Task<DataTable> SelectNewestCommunityPostsAsync(string communityName, PaginationPosts pagination);
    public Task<DataTable> SelectNewestCommunityPostsAsync(string communityName);

    public Task<DataTable> SelectTopCommunityPostsAsync(string communityName, DateTime createdAfter, PaginationPosts pagination);
    public Task<DataTable> SelectTopCommunityPostsAsync(string communityName, DateTime createdAfter);

    public Task<DataTable> SelectCommunityTextPostsAsync(string communityName);
    public Task<DataTable> SelectCommunityLinkPostsAsync(string communityName);

    public Task<DataRow?> SelectPostAsync(Guid postId);
    public Task<DataRow?> SelectPostTextAsync(Guid postId);
    public Task<DataRow?> SelectPostLinkAsync(Guid postId);

    public Task<List<int>> InsertPostAsync(PostText post);
    public Task<List<int>> InsertPostAsync(PostLink post);
}
