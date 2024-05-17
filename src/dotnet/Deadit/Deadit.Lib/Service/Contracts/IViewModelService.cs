using Deadit.Lib.Domain.Dto;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Other;
using Deadit.Lib.Domain.Response;
using Deadit.Lib.Domain.ViewModel;

namespace Deadit.Lib.Service.Contracts;

public interface IViewModelService
{
    public Task<ServiceDataResponse<CommunityPageViewModel>> GetCommunityPageViewModelAsync(string communityName, uint? userId, PostSorting postSorting);
    public Task<ServiceDataResponse<JoinedCommunitiesPageViewModel>> GetJoinedCommunitiesPageViewModelAsync(uint userId);
    public Task<ServiceDataResponse<PostPageViewModel>> GetPostPageViewModelAsync(Guid postId, PostType postType, uint? userId, SortOption sort);
    public Task<ServiceDataResponse<GetCommentsApiViewModel>> GetCommentsApiResponseAsync(Guid postId, uint? clientId, SortOption sort);
    public Task<ServiceDataResponse<GetCommentDto>> GetCommentApiResponseAsync(Guid commentId, uint? clientId);
}


public interface IGetViewModelService<in TArgs, TViewModel>
{
    public Task<ServiceDataResponse<TViewModel>> GetViewModelAsync(TArgs args);
}
