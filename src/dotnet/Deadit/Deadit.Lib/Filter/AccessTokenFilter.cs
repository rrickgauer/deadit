using Deadit.Lib.Domain.Constants;
using Deadit.Lib.Domain.Other;
using Deadit.Lib.Service.Contracts;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Deadit.Lib.Filter;

public class AccessTokenFilter(IApiAccessTokenService tokenService) : IAsyncActionFilter
{
    private readonly IApiAccessTokenService _tokenService = tokenService;

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        await HandleAccessTokenAsync(context);

        await next();
    }

    private async Task HandleAccessTokenAsync(ActionExecutingContext context)
    {

        if (!context.HttpContext.Request.Headers.TryGetValue(CustomRequestHeaders.ApiAccessToken, out var values))
        {
            return;
        }

        if (!Guid.TryParse(values.FirstOrDefault(), out var token))
        {
            return;
        }


        var getToken = await _tokenService.GetTokenAsync(token);

        if (!getToken.Successful)
        {
            return;
        }

        SessionManager mgr = new(context.HttpContext.Session);

        mgr.ClientId = getToken.Data?.UserId;

    }
}
