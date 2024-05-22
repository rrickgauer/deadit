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
public class CommentVoteAuth(IPostService postService, ICommentService commentService) : IAsyncPermissionsAuth<CommentVoteAuthData>
{
    private readonly IPostService _postService = postService;
    private readonly ICommentService _commentService = commentService;
    

    public async Task<ServiceResponse> HasPermissionAsync(CommentVoteAuthData data)
    {
        var getComment = await _commentService.GetCommentAsync(data.CommentId);

        if (!getComment.Successful)
        {
            return new(getComment);
        }

        var comment = NotFoundHttpResponseException.ThrowIfNot<ViewComment>(getComment.Data);

        var postId = NotFoundHttpResponseException.ThrowIfNot<Guid>(comment.CommentPostId);

        var getPost = await _postService.GetPostAsync(postId);

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
