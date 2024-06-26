﻿using Deadit.Lib.Auth;
using Deadit.Lib.Auth.AuthParms;
using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Forms;
using Deadit.Lib.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;


namespace Deadit.Lib.Filter;

public class CommentFilters
{
    public abstract class CommentFilterBase(CommentAuth auth) : IAsyncActionFilter
    {
        public abstract AuthPermissionType PermissionType { get; }

        protected readonly CommentAuth _auth = auth;

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            CommentAuthData authData = GetCommentAuthData(context);

            var authResponse = await _auth.HasPermissionAsync(authData);

            if (!authResponse.Successful)
            {
                context.Result = new BadRequestObjectResult(authResponse);
                return;
            }

            await next();
        }

        protected virtual CommentAuthData GetCommentAuthData(ActionExecutingContext context)
        {
            context.TryGetSessionClientId(out var clientId);


            CommentForm? form = null;

            try
            {
                form = context.GetCommentForm();
            }
            catch(Exception)
            {

            }

            CommentAuthData result = new()
            {
                CommentId = context.GetCommentIdRouteValue(),
                AuthPermissionType = PermissionType,
                UserId = clientId,
                PostId = form?.PostId,
                ParentCommentId = form?.ParentId,
            };

            return result;
        }
    }



    [AutoInject(AutoInjectionType.Scoped, InjectionProject.Always)]
    public class CommentSaveFilter(CommentAuth auth) : CommentFilterBase(auth) 
    {
        public override AuthPermissionType PermissionType => AuthPermissionType.Upsert;
    }


    [AutoInject(AutoInjectionType.Scoped, InjectionProject.Always)]
    public class CommentDeleteFilter(CommentAuth auth) : CommentFilterBase(auth)
    {
        public override AuthPermissionType PermissionType => AuthPermissionType.Delete;
    }


    [AutoInject(AutoInjectionType.Scoped, InjectionProject.Always)]
    public class CommentExistsFilter(CommentAuth auth) : CommentFilterBase(auth)
    {
        public override AuthPermissionType PermissionType => AuthPermissionType.Get;
    }


}
