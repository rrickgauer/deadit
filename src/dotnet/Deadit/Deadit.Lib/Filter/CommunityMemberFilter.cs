﻿using Deadit.Lib.Auth;
using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Deadit.Lib.Filter;

[AutoInject(AutoInjectionType.Scoped, InjectionProject.Always)]
public class CommunityMemberFilter(CreatePostAuth auth) : IAsyncActionFilter
{
    private readonly CreatePostAuth _auth = auth;

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var authResponse = await _auth.HasPermissionAsync(new()
        {
            CommunityName = context.GetCommunityNameRouteValue(),
            UserId = context.GetSessionClientId(),
            FlairPostId = null,
        });

        if (!authResponse.Successful)
        {
            context.Result = new BadRequestObjectResult(authResponse);
            return;
        }

        await next();
    }
}
