using Deadit.Lib.Domain.Forms;
using Deadit.Lib.Domain.Other;
using Deadit.Lib.Domain.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using static Deadit.Lib.Domain.Forms.CreatePostForms;

namespace Deadit.Lib.Utility;

public static class ActionExecutingContextExtensions
{

    public static string GetCommunityNameRouteValueOld(ActionExecutingContext context) => GetRequestRouteValue<string>(context, "communityName");

    
    public static string GetCommunityNameRouteValue(this ActionExecutingContext context)
    {
        return GetRequestRouteValue<string>(context, "communityName");
    }

    public static Guid GetPostIdRouteValue(this ActionExecutingContext context)
    {
        return GetRequestRouteValue<Guid>(context, "postId");
    }

    public static Guid GetCommentIdRouteValue(this ActionExecutingContext context)
    {
        return GetRequestRouteValue<Guid>(context, "commentId");
    }

    public static uint GetFlairPostIdRouteValue(this ActionExecutingContext context)
    {
        return GetRequestRouteValue<uint>(context, "flairId");
    }

    public static uint? GetFlairPostIdRouteValueNull(this ActionExecutingContext context)
    {
        if (context.ActionArguments.ContainsKey("flairId"))
        {
            return context.GetFlairPostIdRouteValue();
        }

        return null;
    }



    /// <summary>
    /// Get the specified request value with the matching key.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="context"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static T GetRequestRouteValue<T>(ActionExecutingContext context, string key)
    {
        var value = (T)context.ActionArguments[key]!;

        return value;
    }

    public static CreateVoteForm GetRequestCreateVoteForm(this ActionExecutingContext context)
    {
        return (CreateVoteForm)context.ActionArguments.Values.First(a => a is CreateVoteForm)!;
    }

    public static CreatePostForm GetCreatePostForm(this ActionExecutingContext context)
    {
        return (CreatePostForm)context.ActionArguments.Values.First(a => a is CreatePostForm)!;
    }

    public static CommentForm GetCommentForm(this ActionExecutingContext context)
    {
        return (CommentForm)context.ActionArguments.Values.First(a => a is CommentForm)!;
    }

    public static FlairPostForm? GetFlairPostForm(this ActionExecutingContext context)
    {
        return context.ActionArguments.Values.FirstOrDefault(a => a is FlairPostForm) as FlairPostForm;
    }

    public static T GetForm<T>(this ActionExecutingContext context)
    {
        return (T)context.ActionArguments.Values.First(a => a is T)!;
    }


    public static SessionManager GetSessionManager(this ActionExecutingContext context)
    {
        return new(context.HttpContext.Session);
    }

    public static uint GetSessionClientId(this ActionExecutingContext context)
    {
        var sessionMgr = context.GetSessionManager();

        var clientId = sessionMgr.ClientId;

        if (!clientId.HasValue)
        {
            throw new UnauthorizedAccessException();
        }

        return clientId.Value;
    }


    public static uint? GetSessionsClientIdNull(this ActionExecutingContext context)
    {
        var sessionMgr = context.GetSessionManager();
        return sessionMgr.ClientId;
    }

    public static bool TryGetSessionClientId(this ActionExecutingContext context, out uint? clientId)
    {
        var sessionMgr = context.GetSessionManager();

        clientId = sessionMgr.ClientId;

        return clientId.HasValue;

    }


    public static void ReturnBadServiceResponse(this ActionExecutingContext context, ServiceResponse serviceResponse)
    {
        context.Result = new BadRequestObjectResult(serviceResponse);
    }
}
