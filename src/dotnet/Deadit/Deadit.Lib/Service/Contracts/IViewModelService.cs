﻿using Deadit.Lib.Domain.Dto;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Response;
using Deadit.Lib.Domain.ViewModel;

namespace Deadit.Lib.Service.Contracts;

public interface IViewModelService
{
    public Task<ServiceDataResponse<CommunityPageViewModel>> GetCommunityPageViewModelAsync(string communityName, uint? userId);
    public Task<ServiceDataResponse<JoinedCommunitiesPageViewModel>> GetJoinedCommunitiesPageViewModelAsync(uint userId);
    public Task<ServiceDataResponse<PostPageViewModel>> GetPostPageViewModelAsync(Guid postId, PostType postType, uint? userId);

    public Task<ServiceDataResponse<GetCommentsApiViewModel>> GetCommentsApiResponseAsync(Guid postId, uint? clientId);
    public Task<ServiceDataResponse<GetCommentDto>> GetCommentApiResponseAsync(Guid commentId, uint? clientId);
}

