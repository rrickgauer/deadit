﻿using Deadit.Lib.Service.Contracts;
using Deadit.Lib.Service.Implementations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Deadit.Lib.Filter;

public class CommunityNameExistsFilter : IAsyncActionFilter
{
    private readonly ICommunityService _communityService;

    public CommunityNameExistsFilter(ICommunityService communityService)
    {
        _communityService = communityService;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        string communityName = FilterService.GetCommunityNameRouteValue(context);

        var getCommunityResponse = await _communityService.GetCommunityAsync(communityName);

        if (!getCommunityResponse.HasData)
        {
            context.Result = new NotFoundObjectResult(null);
            return;
        }

        await next();
    }
}
