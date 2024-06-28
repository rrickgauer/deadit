using Deadit.Lib.Auth;
using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Utility;
using Microsoft.AspNetCore.Mvc.Filters;
using static Deadit.Lib.Domain.Forms.CreatePostForms;

namespace Deadit.Lib.Filter;

[AutoInject(AutoInjectionType.Scoped, InjectionProject.WebGui)]
public class CreatePostFilter(CreatePostAuth auth) : IAsyncActionFilter
{
    private readonly CreatePostAuth _auth = auth;

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        CreatePostForm form = context.GetCreatePostForm();

        PostType postType = PostType.Link;

        string? content = null;

        if (form is CreateTextPostForm textForm)
        {
            postType = PostType.Text;
            content = textForm.Content;
        }

        var hasAuth = await _auth.HasPermissionAsync(new()
        {
            CommunityName = form.CommunityName,
            UserId = context.GetSessionClientId(),
            PostType = postType,
            TextPostContent = content,
            FlairPostId = form.FlairPostId,
        });

        if (!hasAuth.Successful)
        {
            context.ReturnBadServiceResponse(hasAuth);
            return;
        }


        await next();
    }
}
