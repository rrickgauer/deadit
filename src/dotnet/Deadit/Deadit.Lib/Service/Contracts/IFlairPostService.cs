using Deadit.Lib.Domain.Model;
using Deadit.Lib.Domain.Response;
using Deadit.Lib.Domain.TableView;

namespace Deadit.Lib.Service.Contracts;


public interface IFlairPostService
{
    public Task<ServiceDataResponse<List<ViewFlairPost>>> GetFlairPostsAsync(uint communityId);
    public Task<ServiceDataResponse<List<ViewFlairPost>>> GetFlairPostsAsync(string communityName);
    public Task<ServiceDataResponse<ViewFlairPost>> GetFlairPostAsync(uint flairId);
    public Task<ServiceDataResponse<ViewFlairPost>> CreateFlairPostAsync(FlairPost flair);
    public Task<ServiceDataResponse<ViewFlairPost>> UpdateFlairPostAsync(FlairPost flair);
    public Task<ServiceResponse> DeleteFlairPostAsync(uint flairId);


}
