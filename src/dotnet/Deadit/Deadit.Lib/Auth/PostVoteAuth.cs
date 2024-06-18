using Deadit.Lib.Auth.AuthParms;
using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Errors;
using Deadit.Lib.Domain.Response;
using Deadit.Lib.Domain.TableView;
using Deadit.Lib.Service.Contracts;



namespace Deadit.Lib.Auth;


[AutoInject(AutoInjectionType.Scoped, InjectionProject.Always)]
public class PostVoteAuth(IPostService postService) : IAsyncPermissionsAuth<PostVoteAuthData>
{
    protected readonly IPostService _postService = postService;

    public virtual async Task<ServiceResponse> HasPermissionAsync(PostVoteAuthData data)
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

        if (post.PostDeletedOn.HasValue)
        {
            throw new ForbiddenHttpResponseException();
        }

        return new();
    }

}
