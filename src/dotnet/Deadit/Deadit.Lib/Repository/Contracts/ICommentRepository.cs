using Deadit.Lib.Domain.Model;
using System.Data;

namespace Deadit.Lib.Repository.Contracts;

public interface ICommentRepository
{
    public Task<DataTable> SelectAllPostCommentsAsync(Guid postId);
    public Task<DataRow?> SelectCommentAsync(Guid commentId);
    public Task<int> SaveCommentAsync(Comment comment);
}
