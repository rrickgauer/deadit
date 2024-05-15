using Deadit.Lib.Domain.Model;
using Deadit.Lib.Domain.Parms;
using Deadit.Lib.Domain.Response;
using Deadit.Lib.Domain.TableView;

namespace Deadit.Lib.Service.Contracts;

public interface ICommentService
{
    public Task<ServiceDataResponse<List<ViewComment>>> GetCommentsNestedAsync(GetCommentsParms args);
    public Task<ServiceDataResponse<List<ViewComment>>> GetCommentsAsync(GetCommentsParms args);
    public Task<ServiceDataResponse<ViewComment>> GetCommentAsync(Guid commentId);
    public Task<ServiceDataResponse<ViewComment>> SaveCommentAsync(Comment comment);
    public Task<ServiceResponse> DeleteCommentAsync(Guid commentId);
}
