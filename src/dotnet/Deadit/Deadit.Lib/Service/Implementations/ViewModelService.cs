using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Errors;
using Deadit.Lib.Domain.Response;
using Deadit.Lib.Domain.ViewModel;
using Deadit.Lib.Service.Contracts;
using System.Net;

namespace Deadit.Lib.Service.Implementations;

[AutoInject(AutoInjectionType.Scoped, InjectionProject.WebGui, InterfaceType = typeof(IViewModelService))]
public class ViewModelService : IViewModelService
{

    private readonly ICommunityService _communityService;
    private readonly ICommunityMemberService _memberService;

    public ViewModelService(ICommunityService communityService, ICommunityMemberService memberService)
    {
        _communityService = communityService;
        _memberService = memberService;
    }

    public async Task<ServiceDataResponse<CommunityPageViewModel>> GetCommunityPageViewModelAsync(string communityName, uint? userId)
    {
        ServiceDataResponse<CommunityPageViewModel> result = new();

        var community = (await _communityService.GetCommunityAsync(communityName)).Data;

        if (community == null)
        {
            throw new NotFoundHttpResponseException();
        }

        var communityId = community?.CommunityId;

        bool isMember = false;

        if (userId.HasValue && communityId.HasValue)
        {
            isMember = (await _memberService.IsMemberAsync(userId.Value, communityId.Value)).Data;
        }

        result.Data = new()
        {
            Community = community,
            IsMember = isMember,
        };

        return result;
    }
}

