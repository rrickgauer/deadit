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
public class ModifyPostAuth(IPostService postService, IHttpContextAccessor httpContextAccessor) : IAsyncPermissionsAuth<ModifyPostAuthData>
{
    private readonly IPostService _postService = postService;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private HttpContext? _context => _httpContextAccessor.HttpContext?.Request.HttpContext;

    public async Task<ServiceResponse> HasPermissionAsync(ModifyPostAuthData data)
    {
        var getPost = await _postService.GetPostAsync(data.PostId);

        if (!getPost.Successful)
        {
            return getPost;
        }

        if (getPost.Data is not ViewPost post)
        {
            throw new NotFoundHttpResponseException();
        }

        if (post.PostAuthorId != data.ClientId)
        {
            throw new ForbiddenHttpResponseException();
        }

        if (post.PostDeletedOn.HasValue)
        {
            throw new ForbiddenHttpResponseException();
        }

        if (_context != null)
        {
            HttpRequestItems items = new(_context)
            {
                Post = post,
                CommunityId = post.CommunityId,
            };
        }

        return new();
    }
}
