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
public class FlairPostAuth(ICommunityService communityService, IFlairPostService flairPostService, IHttpContextAccessor contextAccessor) : IAsyncPermissionsAuth<UpdateFlairPostAuthData>
{
    private readonly ICommunityService _communityService = communityService;
    private readonly IFlairPostService _flairPostService = flairPostService;
    private readonly IHttpContextAccessor _contextAccessor = contextAccessor;

    public async Task<ServiceResponse> HasPermissionAsync(UpdateFlairPostAuthData data)
    {
        
        try
        {
            return data.ValidationLevel switch
            {
                FlairPostAuthValidationLevel.Update => await CanModifyAsync(data),
                FlairPostAuthValidationLevel.Insert => await CanInsertAsync(data),
                FlairPostAuthValidationLevel.Delete => await CanDeleteFlairAsync(data),
                _                                   => throw new NotImplementedException($"{data.ValidationLevel}"),
            };
        }
        catch (ServiceResponseException ex)
        {
            return ex;
        }
    }



    /// <summary>
    /// Check if client can update/delete flair
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    /// <exception cref="ForbiddenHttpResponseException"></exception>
    private async Task<ServiceResponse> CanModifyAsync(UpdateFlairPostAuthData data)
    {
        var flair = await GetFlairAsync(data);

        if (flair.CommunityName != data.FlairForm?.CommunityName)
        {
            throw new ForbiddenHttpResponseException();
        }

        if (flair.CommunityOwnerId != data.ClientId)
        {
            throw new ForbiddenHttpResponseException();
        }

        HttpRequestItems.StoreFlairPost(_contextAccessor, flair);
        HttpRequestItems.StoreCommunityId(_contextAccessor.HttpContext, flair.CommunityId);

        return new();

    }
    

    private async Task<ServiceResponse> CanDeleteFlairAsync(UpdateFlairPostAuthData data)
    {
        var flair = await GetFlairAsync(data);

        if (flair.CommunityOwnerId != data.ClientId)
        {
            throw new ForbiddenHttpResponseException();
        }

        return new();
    }

    private async Task<ViewFlairPost> GetFlairAsync(UpdateFlairPostAuthData data)
    {
        if (data.FlairId is not uint flairId)
        {
            throw new ArgumentNullException(nameof(data.FlairId));
        }

        var getFlair = await _flairPostService.GetFlairPostAsync(flairId);

        getFlair.ThrowIfError();

        var flair = NotFoundHttpResponseException.ThrowIfNot<ViewFlairPost>(getFlair.Data);

        return flair;
    }


    private async Task<ServiceResponse> CanInsertAsync(UpdateFlairPostAuthData data)
    {
        if (data.FlairForm?.CommunityName is not string communityName)
        {
            throw new ArgumentNullException(nameof(UpdateFlairPostAuthData.FlairForm));
        }

        var community = await GetCommunityAsync(communityName);

        if (community.CommunityOwnerId != data.ClientId)
        {
            throw new ForbiddenHttpResponseException();
        }

        HttpRequestItems.StoreCommunityId(_contextAccessor.HttpContext, community.CommunityId);

        return new();
    }

    private async Task<ViewCommunity> GetCommunityAsync(string communityName)
    {
        var getCommunity = await _communityService.GetCommunityAsync(communityName);

        getCommunity.ThrowIfError();

        return NotFoundHttpResponseException.ThrowIfNot<ViewCommunity>(getCommunity.Data);
    }






}
