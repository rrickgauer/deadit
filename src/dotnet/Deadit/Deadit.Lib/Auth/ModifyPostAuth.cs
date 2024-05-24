using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Errors;
using Deadit.Lib.Domain.Response;
using Deadit.Lib.Domain.TableView;
using Deadit.Lib.Service.Contracts;
using static Deadit.Lib.Auth.AuthParms;
using static Deadit.Lib.Auth.PermissionContracts;

namespace Deadit.Lib.Auth;

[AutoInject(AutoInjectionType.Scoped, InjectionProject.Always)]
public class ModifyPostAuth(IPostService postService) : IAsyncPermissionsAuth<ModifyPostAuthData>
{
    private readonly IPostService _postService = postService;

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

        if (post.CommunityName != data.CommunityName)
        {
            throw new NotFoundHttpResponseException();
        }

        if (post.PostDeletedOn.HasValue)
        {
            throw new ForbiddenHttpResponseException();
        }

        return new();
    }
}
