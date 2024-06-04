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
public class CommentAuth(ICommentService commentService, IPostService postService) : IAsyncPermissionsAuth<CommentAuthData>
{
    private readonly ICommentService _commentService = commentService;
    private readonly IPostService _postService = postService;

    public async Task<ServiceResponse> HasPermissionAsync(CommentAuthData data)
    {
        try
        {
            var comment = await GetCommentAsync(data.CommentId);

            return data.AuthPermissionType switch
            {
                AuthPermissionType.Delete => await ValidateDelete(data, comment),
                AuthPermissionType.Upsert => await ValidateUpsert(data, comment),
                AuthPermissionType.Get    => await ValidateGet(data, comment),
                _                         => throw new NotImplementedException(),
            };
        }
        catch(ServiceResponseException ex)
        {
            return new(ex);
        }
    }



    private async Task<ServiceResponse> ValidateDelete(CommentAuthData data, ViewComment? comment)
    {

        if (comment == null)
        {
            throw new NotFoundHttpResponseException();
        }

        if (comment.CommentAuthorId != data.UserId)
        {
            throw new ForbiddenHttpResponseException();
        }

        var post = await GetPostAsync(data);

        if (post.PostDeletedOn.HasValue)
        {
            throw new ForbiddenHttpResponseException();
        }

        return new();
    }


    private async Task<ServiceResponse> ValidateUpsert(CommentAuthData data, ViewComment? comment)
    {
        // ensure the post exists
        var post = await GetPostAsync(data);

        // can't comment on deleted posts
        if (post.PostDeletedOn.HasValue)
        {
            return new(ErrorCode.CommentPostDeleted);
        }

        // can't comment on locked posts
        if (post.PostIsLocked)
        {
            return new(ErrorCode.CommentPostLocked);
        }

        // can't comment on removed posts
        if (post.PostIsRemoved)
        {
            return new(ErrorCode.CommentPostRemoved);
        }

        // comment is a new one so we don't have to check anything further
        if (comment == null)
        {
            return new();
        }

        // comment exists, so we are updating it
        // make sure that the comment's author id is the same as the client's
        if (comment.CommentAuthorId != data.UserId)
        {
            throw new ForbiddenHttpResponseException();
        }

        // if this comment is a reply to another comment
        if (comment.CommentParentId is Guid parentId)
        {
            // make sure the parent exists
            var parentComment = await GetCommentAsync(parentId);

            if (parentComment == null)
            {
                return new(ErrorCode.CommentInvalidParentId);
            }

            // check if the parent comment is locked
            if (parentComment.CommentLockedOn.HasValue)
            {
                return new(ErrorCode.CommentParentCommentIsLocked);
            }
        }

        return new();
    }



    private async Task<ServiceResponse> ValidateGet(CommentAuthData data, ViewComment? comment)
    {
        if (comment == null)
        {
            throw new NotFoundHttpResponseException();
        }

        if (comment.CommentId is not Guid)
        {
            throw new NotFoundHttpResponseException();
        }

        //var post = await GetPostAsync(data);

        return new();
    }


    private async Task<ViewPost> GetPostAsync(CommentAuthData data)
    {
        var getPost = await _postService.GetPostAsync(data.PostId);

        if (!getPost.Successful)
        {
            throw new ServiceResponseException(getPost);
        }

        if (getPost.Data is not ViewPost post)
        {
            throw new NotFoundHttpResponseException();
        }

        if (post.CommunityName != data.CommunityName)
        {
            throw new NotFoundHttpResponseException();
        }

        return post;
    }


    private async Task<ViewComment?> GetCommentAsync(Guid commentId)
    {
        var getComment = await _commentService.GetCommentAsync(commentId);

        getComment.ThrowIfError();

        return getComment.Data;
    }

}
