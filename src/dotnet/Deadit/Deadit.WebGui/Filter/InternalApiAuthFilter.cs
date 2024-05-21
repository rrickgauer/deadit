using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Constants;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Errors;
using Deadit.Lib.Domain.Other;
using Deadit.Lib.Service.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace Deadit.WebGui.Filter;

[AutoInject(AutoInjectionType.Scoped, InjectionProject.WebGui)]
public class InternalApiAuthFilter(IApiAccessTokenService tokenService) : IAsyncActionFilter
{
    private readonly IApiAccessTokenService _tokenService = tokenService;

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        await HandleAccessTokenAsync(context);

        SessionManager sessionManager = new(context.HttpContext.Session);

        if (!sessionManager.IsClientAuthorized())
        {
            throw new HttpResponseException(HttpStatusCode.Forbidden);
        }

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
