using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Model;
using System.Data;

namespace Deadit.Lib.Repository.Contracts;

public interface IPostRepository
{
    public Task<DataTable> SelectAllCommunityPostsAsync(string communityName);
    public Task<DataTable> SelectAllCommunityTextPostsAsync(string communityName);
    public Task<DataTable> SelectAllCommunityLinkPostsAsync(string communityName);

    public Task<DataTable> SelectAllTopCommunityPostsAsync(string communityName, DateTime createdOn);

    public Task<DataRow?> SelectPostAsync(Guid postId);
    public Task<DataRow?> SelectTextAsync(Guid postId);
    public Task<DataRow?> SelectLinkAsync(Guid postId);

    public Task<List<int>> InsertPostAsync(PostText post);
    public Task<List<int>> InsertPostAsync(PostLink post);
}
