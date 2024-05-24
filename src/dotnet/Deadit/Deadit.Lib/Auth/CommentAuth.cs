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
            var getComment = await _commentService.GetCommentAsync(data.CommentId);

            if (!getComment.Successful)
            {
                return new(getComment);
            }

            return data.AuthPermissionType switch
            {
                AuthPermissionType.Delete => await ValidateDelete(data, getComment),
                AuthPermissionType.Upsert => await ValidateUpsert(data, getComment),
                AuthPermissionType.Get    => await ValidateGet(data, getComment),
                _                         => throw new NotImplementedException(),
            };
        }
        catch(ServiceResponseException ex)
        {
            return new(ex);
        }
    }



    private async Task<ServiceResponse> ValidateDelete(CommentAuthData data, ServiceDataResponse<ViewComment> getCommet)
    {
        var comment = NotFoundHttpResponseException.ThrowIfNot<ViewComment>(getCommet.Data);

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



    private static ServiceResponse ValidateSave(CommentAuthData data, ServiceDataResponse<ViewComment> getCommet)
    {
        if (getCommet.Data is not ViewComment comment)
        {
            throw new NotFoundHttpResponseException();
        }

        if (comment.CommentAuthorId != data.UserId)
        {
            throw new ForbiddenHttpResponseException();
        }

        return new();
    }

    private static ServiceResponse ValidateCreate(CommentAuthData data, ServiceDataResponse<ViewComment> getCommet)
    {
        if (getCommet.Data is ViewComment)
        {
            throw new ForbiddenHttpResponseException();
        }

        return new();
    }


    private async Task<ServiceResponse> ValidateUpsert(CommentAuthData data, ServiceDataResponse<ViewComment> getCommet)
    {
        if (getCommet.Data is not ViewComment comment)
        {
            return new();
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



    private async Task<ServiceResponse> ValidateGet(CommentAuthData data, ServiceDataResponse<ViewComment> getComment)
    {
        if (getComment.Data is not ViewComment comment)
        {
            throw new NotFoundHttpResponseException();
        }

        if (comment.CommentId is not Guid)
        {
            throw new NotFoundHttpResponseException();
        }

        var post = await GetPostAsync(data);

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

}
