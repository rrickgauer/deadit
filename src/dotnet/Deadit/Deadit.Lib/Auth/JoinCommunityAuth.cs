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
public class JoinCommunityAuth(ICommunityService communityService, IHttpContextAccessor accessor) : IAsyncPermissionsAuth<JoinCommunityAuthData>
{
    private readonly ICommunityService _communityService = communityService;
    private readonly IHttpContextAccessor _accessor = accessor;


    public async Task<ServiceResponse> HasPermissionAsync(JoinCommunityAuthData data)
    {
        try
        {
            var community = await GetCommunityAsync(data);

            if (!community.CommunityAcceptingNewMembers)
            {
                throw new ForbiddenHttpResponseException();
            }

            HttpRequestItems.StoreCommunityId(_accessor.HttpContext?.Request.HttpContext, community.CommunityId);

            return new();
        }
        catch (ServiceResponseException ex)
        {
            return ex;
        }
    }


    private async Task<ViewCommunity> GetCommunityAsync(JoinCommunityAuthData data)
    {
        var getCommunity = await _communityService.GetCommunityAsync(data.CommunityName);

        getCommunity.ThrowIfError();

        if (getCommunity.Data is not ViewCommunity community)
        {
            throw new NotFoundHttpResponseException();
        }

        return community;
    }
}
