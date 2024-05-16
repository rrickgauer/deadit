using Deadit.Lib.Auth;
using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using static Deadit.Lib.Auth.AuthParms;

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

            CommentAuthData result = new()
            {
                CommentId = context.GetCommentIdRouteValue(),
                AuthPermissionType = PermissionType,
                UserId = clientId,
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
