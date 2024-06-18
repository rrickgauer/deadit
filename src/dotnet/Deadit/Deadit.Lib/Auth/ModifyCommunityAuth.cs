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
public class ModifyCommunityAuth(ICommunityService communityService, IHttpContextAccessor contextAccessor) : IAsyncPermissionsAuth<ModifyCommunityData>
{
    private readonly ICommunityService _communityService = communityService;
    private readonly IHttpContextAccessor _contextAccessor = contextAccessor;
    private HttpContext? _context => _contextAccessor.HttpContext?.Request.HttpContext;

    public async Task<ServiceResponse> HasPermissionAsync(ModifyCommunityData data)
    {
        var getCommunity = await _communityService.GetCommunityAsync(data.CommunityName);

        if (!getCommunity.Successful)
        {
            return getCommunity;
        }

        var community = NotFoundHttpResponseException.ThrowIfNot<ViewCommunity>(getCommunity.Data);

        if (community.CommunityOwnerId != data.ClientId)
        {
            throw new ForbiddenHttpResponseException();
        }

        HttpRequestItems.StoreCommunityId(_context, community.CommunityId);

        return new();
    }
}
