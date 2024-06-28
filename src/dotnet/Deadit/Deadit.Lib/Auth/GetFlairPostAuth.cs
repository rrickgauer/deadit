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
public class GetFlairPostAuth(IFlairPostService flairService, IHttpContextAccessor accessor) : IAsyncPermissionsAuth<GetFlairPostAuthData>
{
    private readonly IFlairPostService _flairService = flairService;
    private readonly IHttpContextAccessor _accessor = accessor;

    public async Task<ServiceResponse> HasPermissionAsync(GetFlairPostAuthData data)
    {
        try
        {
            var flair = await GetFlairAsync(data);
            HttpRequestItems.StoreFlairPost(_accessor, flair);

            return new();
        }
        catch(ServiceResponseException ex)
        {
            return ex;
        }
    }


    private async Task<ViewFlairPost> GetFlairAsync(GetFlairPostAuthData data)
    {
        var getFlair = await _flairService.GetFlairPostAsync(data.FlairPostId);

        getFlair.ThrowIfError();

        var flair = NotFoundHttpResponseException.ThrowIfNot<ViewFlairPost>(getFlair.Data);

        return flair;
    }
}
