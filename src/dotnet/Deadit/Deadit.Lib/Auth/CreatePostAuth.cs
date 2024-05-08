using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Errors;
using Deadit.Lib.Domain.Response;
using Deadit.Lib.Domain.TableView;
using Deadit.Lib.Service.Contracts;
using static Deadit.Lib.Auth.AuthParms;
using static Deadit.Lib.Auth.PermissionContracts;

namespace Deadit.Lib.Auth;


[AutoInject(AutoInjectionType.Scoped, InjectionProject.Always)]
public class CreatePostAuth(ICommunityMemberService communityMemberService, ICommunityService communityService) : IAsyncPermissionsAuth<CreatePostAuthData>
{
    private readonly ICommunityMemberService _communityMemberService = communityMemberService;
    private readonly ICommunityService _communityService = communityService;

    public async Task<ServiceResponse> HasPermissionAsync(CreatePostAuthData data)
    {
        // check if user is the owner
        var getCommunity = await _communityService.GetCommunityAsync(data.CommunityName);

        if (!getCommunity.Successful)
        {
            return getCommunity;
        }

        if (getCommunity.Data is not ViewCommunity community)
        {
            throw new NotFoundHttpResponseException();
        }
        
        if (community.CommunityOwnerId == data.UserId)
        {
            return getCommunity;
        }
        

        // check if the user is member
        var getMembership = await _communityMemberService.GetMembershipAsync(data.UserId, data.CommunityName);

        if (getMembership.Data is not ViewCommunityMembership membership)
        {
            throw new ForbiddenHttpResponseException();
        }

        return getMembership;
    }
}
