using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Model;
using System.Data;

namespace Deadit.Lib.Repository.Contracts;

public interface ICommentRepository
{
    public Task<DataTable> SelectAllPostCommentsAsync(Guid postId, SortOption sortBy);
    public Task<DataRow?> SelectCommentAsync(Guid commentId);
    public Task<int> SaveCommentAsync(Comment comment);
    public Task<int> DeleteCommentAsync(Guid commentId);
}
