using Deadit.Lib.Domain.Model;
using System.Data;

namespace Deadit.Lib.Repository.Contracts;

public interface IPostRepository
{
    public Task<DataTable> SelectAllCommunityAsync(string communityName);
    public Task<DataTable> SelectAllCommunityTextAsync(string communityName);
    public Task<DataTable> SelectAllCommunityLinkAsync(string communityName);

    public Task<DataRow?> SelectPostAsync(Guid postId);
    public Task<DataRow?> SelectTextAsync(Guid postId);
    public Task<DataRow?> SelectLinkAsync(Guid postId);

    public Task<List<int>> InsertPostAsync(PostText post);
    public Task<List<int>> InsertPostAsync(PostLink post);
}
