using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Errors;
using Deadit.Lib.Domain.Other;
using Deadit.Lib.Domain.Response;
using Deadit.Lib.Domain.TableView;
using Deadit.Lib.Service.Contracts;
using Microsoft.AspNetCore.Http;
using static Deadit.Lib.Auth.AuthParms;
using static Deadit.Lib.Auth.PermissionContracts;

namespace Deadit.Lib.Auth;



[AutoInject(AutoInjectionType.Scoped, InjectionProject.Always)]
public class GetPostAuth(IPostService postService, IHttpContextAccessor contextAccessor) : IAsyncPermissionsAuth<GetPostAuthData>
{
    private readonly IPostService _postService = postService;
    private readonly IHttpContextAccessor _contextAccessor = contextAccessor;

    private HttpContext? Context => _contextAccessor.HttpContext?.Request.HttpContext;

    public async Task<ServiceResponse> HasPermissionAsync(GetPostAuthData data)
    {
        // make sure post exists
        var getPost = await _postService.GetPostAsync(data.PostId);

        if (!getPost.Successful)
        {
            return new(getPost);
        }

        if (getPost.Data is not ViewPost post)
        {
            throw new NotFoundHttpResponseException();
        }

        // make sure the post belongs to the specified community
        if (!data.CommunityName.Equals(post.CommunityName, StringComparison.OrdinalIgnoreCase))
        {
            throw new NotFoundHttpResponseException();
        }

        // store the post in request data
        if (Context != null)
        {
            HttpRequestItems dataDict = new(Context)
            {
                Post = post,
                
            };
        }


        return new();
    }
}
