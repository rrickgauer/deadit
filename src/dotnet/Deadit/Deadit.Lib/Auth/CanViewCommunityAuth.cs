using Deadit.Lib.Auth.AuthParms;
using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Errors;
using Deadit.Lib.Domain.Other;
using Deadit.Lib.Domain.Response;
using Deadit.Lib.Domain.TableView;
using Deadit.Lib.Service.Contracts;
using Microsoft.AspNetCore.Http;

namespace Deadit.Lib.Auth;

[AutoInject(AutoInjectionType.Scoped, InjectionProject.WebGui)]
public class CanViewCommunityAuth(ICommunityService communityService, ICommunityMemberService communityMemberService, IHttpContextAccessor accessor) : IAsyncPermissionsAuth<CanViewCommunityAuthData>
{
    private readonly ICommunityService _communityService = communityService;
    private readonly ICommunityMemberService _communityMemberService = communityMemberService;
    private readonly IHttpContextAccessor _accessor = accessor;
    private HttpContext? _context => _accessor.HttpContext?.Request.HttpContext;

    public async Task<ServiceResponse> HasPermissionAsync(CanViewCommunityAuthData data)
    {
        try
        {
            var community = await GetCommunityAsync(data);

            // since community is public, we don't need to check anything else
            if (community.CommunityType == CommunityType.Public)
            {
                return new();
            }

            if (data.ClientId is not uint clientId)
            {
                return new(ErrorCode.CommunitySettingsPrivateCommunityAccessAttempt);
            }

            // check if client is the owner of the community
            if (community.CommunityOwnerId == clientId)
            {
                return new();
            }

            var getMembership = await _communityMemberService.GetMembershipAsync(clientId, data.CommunityName);

            bool isMember = getMembership.Data != null;

            if (!isMember)
            {
                return new(ErrorCode.CommunitySettingsPrivateCommunityAccessAttempt);
            }

            return new();
        }
        catch (ServiceResponseException ex)
        {
            return ex;
        }
    }


    private async Task<ViewCommunity> GetCommunityAsync(CanViewCommunityAuthData data)
    {
        var getCommunity = await _communityService.GetCommunityAsync(data.CommunityName);

        getCommunity.ThrowIfError();

        if (getCommunity.Data is not ViewCommunity community)
        {
            throw new NotFoundHttpResponseException();
        }

        HttpRequestItems.StoreCommunityId(_context, community.CommunityId);

        return community;
    }
}
