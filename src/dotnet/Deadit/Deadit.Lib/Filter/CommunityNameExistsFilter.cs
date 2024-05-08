using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Errors;
using Deadit.Lib.Domain.Other;
using Deadit.Lib.Service.Contracts;
using Deadit.Lib.Utility;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Deadit.Lib.Filter;

[AutoInject(AutoInjectionType.Scoped, InjectionProject.WebGui)]
public class CommunityNameExistsFilter(ICommunityService communityService) : IAsyncActionFilter
{
    private readonly ICommunityService _communityService = communityService;

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        string communityName = ActionExecutingContextExtensions.GetCommunityNameRouteValue(context);

        var getCommunityResponse = await _communityService.GetCommunityAsync(communityName);

        if (!getCommunityResponse.HasData)
        {
            throw new NotFoundHttpResponseException();
        }

        HttpRequestItems dataDict = new(context)
        {
            CommunityId = getCommunityResponse?.Data?.CommunityId
        };


        await next();
    }
}
