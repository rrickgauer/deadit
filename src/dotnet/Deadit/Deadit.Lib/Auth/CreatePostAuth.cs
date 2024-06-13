using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Errors;
using Deadit.Lib.Domain.Other;
using Deadit.Lib.Domain.Response;
using Deadit.Lib.Domain.TableView;
using Deadit.Lib.Service.Contracts;
using Microsoft.AspNetCore.Http;
using static Deadit.Lib.Auth.AuthParms;
using static Deadit.Lib.Auth.PermissionContracts;

namespace Deadit.Lib.Auth;


[AutoInject(AutoInjectionType.Scoped, InjectionProject.Always)]
public class CreatePostAuth(ICommunityMemberService communityMemberService, ICommunityService communityService, IHttpContextAccessor accessor) : IAsyncPermissionsAuth<CreatePostAuthData>
{
    private readonly ICommunityMemberService _communityMemberService = communityMemberService;
    private readonly ICommunityService _communityService = communityService;
    private readonly IHttpContextAccessor _accessor = accessor;
    private HttpContext? _context => _accessor.HttpContext?.Request.HttpContext;

    public async Task<ServiceResponse> HasPermissionAsync(CreatePostAuthData data)
    {
        try
        {
            // ensure the community exists
            var community = await GetCommunityAsync(data);

            // if this is a text post, make sure it doesn't violate the community text post rule
            HandleTextPostRule(community, data);

            // check if client is the owner
            if (community.CommunityOwnerId == data.UserId)
            {
                return new();
            }

            // client is not the owner, so check if they are a community member
            var membership = await GetMembershipAsync(data);

            return new();
        }
        catch(ServiceResponseException ex)
        {
            return ex;
        }
    }


    private async Task<ViewCommunity> GetCommunityAsync(CreatePostAuthData args)
    {
        var getCommunity = await _communityService.GetCommunityAsync(args.CommunityName);

        getCommunity.ThrowIfError();

        if (getCommunity.Data is not ViewCommunity community)
        {
            throw new NotFoundHttpResponseException();
        }

        HttpRequestItems.StoreCommunityId(_context, community.CommunityId);

        return community;
    }


    private async Task<ViewCommunityMembership> GetMembershipAsync(CreatePostAuthData args)
    {
        // check if the user is member
        var getMembership = await _communityMemberService.GetMembershipAsync(args.UserId, args.CommunityName);

        getMembership.ThrowIfError();

        if (getMembership.Data is not ViewCommunityMembership membership)
        {
            throw new ForbiddenHttpResponseException();
        }

        return membership;
    }

    private static void HandleTextPostRule(ViewCommunity community, CreatePostAuthData args)
    {
        if (args.PostType != PostType.Text)
        {
            return;
        }

        switch(community.CommunityTextPostBodyRule)
        {
            case TextPostBodyRule.Required:
                if (string.IsNullOrWhiteSpace(args.TextPostContent))
                {
                    throw new ServiceResponseException(ErrorCode.PostTextPostContentRequired);
                }

                break;

            case TextPostBodyRule.NotAllowed:

                if (!string.IsNullOrEmpty(args.TextPostContent))
                {
                    throw new ServiceResponseException(ErrorCode.PostTextPostContentNotAllowed);
                }

                break;
        }

    }
}
