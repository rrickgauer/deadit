using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Errors;
using Deadit.Lib.Domain.Model;
using Deadit.Lib.Domain.Other;
using Deadit.Lib.Domain.Response;
using Deadit.Lib.Domain.TableView;
using Deadit.Lib.Service.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.Xml.Linq;
using static Deadit.Lib.Auth.AuthParms;
using static Deadit.Lib.Auth.PermissionContracts;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Deadit.Lib.Auth;


[AutoInject(AutoInjectionType.Scoped, InjectionProject.Always)]
public class CommentAuth(ICommentService commentService, IPostService postService, ICommunityMemberService communityMemberService, IHttpContextAccessor httpContextAccessor) : IAsyncPermissionsAuth<CommentAuthData>
{
    private readonly ICommentService _commentService = commentService;
    private readonly IPostService _postService = postService;
    private readonly ICommunityMemberService _communityMemberService = communityMemberService;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    
    private HttpContext? _context => _httpContextAccessor.HttpContext?.Request.HttpContext;

    public async Task<ServiceResponse> HasPermissionAsync(CommentAuthData data)
    {
        try
        {
            var comment = await GetCommentAsync(data.CommentId);

            return data.AuthPermissionType switch
            {
                AuthPermissionType.Delete => await ValidateDelete(data, comment),
                AuthPermissionType.Get    => await ValidateGet(data, comment),
                AuthPermissionType.Upsert => await ValidateUpsert(data, comment),
                _                         => throw new NotImplementedException(),
            };
        }
        catch(ServiceResponseException ex)
        {
            return new(ex);
        }
    }

    private async Task<ViewComment?> GetCommentAsync(Guid commentId)
    {
        var getComment = await _commentService.GetCommentAsync(commentId);

        getComment.ThrowIfError();

        return getComment.Data;
    }



    private async Task<ServiceResponse> ValidateDelete(CommentAuthData data, ViewComment? comment)
    {
        // ensure comment exists
        if (comment == null)
        {
            throw new NotFoundHttpResponseException();
        }

        // ensure client is the comment author
        if (comment.CommentAuthorId != data.UserId)
        {
            throw new ForbiddenHttpResponseException();
        }

        if (comment.CommentPostId is not Guid postId)
        {
            throw new NotFoundHttpResponseException();
        }

        await GetPostAsync(postId);

        return new();
    }

    private async Task<ServiceResponse> ValidateGet(CommentAuthData data, ViewComment? comment)
    {
        // ensure comment exists
        if (comment?.CommentId is not Guid)
        {
            throw new NotFoundHttpResponseException();
        }

        // store post in request data

        if (comment.CommentPostId is not Guid postId)
        {
            throw new NotFoundHttpResponseException();
        }

        await GetPostAsync(postId);

        return new();
    }


    private async Task<ServiceResponse> ValidateUpsert(CommentAuthData data, ViewComment? comment)
    {
        // ensure the post exists

        if (data.PostId is not Guid postId)
        {
            throw new NotFoundHttpResponseException();
        }

        var post = await GetPostAsync(postId);

        // ensure the post can be commented on
        var validatePost = ValidatePost(post);

        if (!validatePost.Successful)
        {
            return validatePost;
        }

        // ensure post has community id
        if (post.PostCommunityId is not uint communityId)
        {
            throw new NotFoundHttpResponseException();
        }

        // store it
        HttpRequestItems.StoreCommunityId(_context, communityId);

        // comment exists, so we are updating it
        if (comment != null)
        {
            return ValidateUpdateCommentAsync(data, comment);
        }

        // comment is a new one
        return await ValidateNewCommentAsync(data, communityId, post.CommunityOwnerId);
    }


    private ServiceResponse ValidatePost(ViewPost post)
    {
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

        return new();
    }

    private ServiceResponse ValidateUpdateCommentAsync(CommentAuthData data, ViewComment comment)
    {
        // make sure that the comment's author id is the same as the client's
        if (comment.CommentAuthorId != data.UserId)
        {
            throw new ForbiddenHttpResponseException();
        }

        return new();
    }


    private async Task<ServiceResponse> ValidateNewCommentAsync(CommentAuthData data, uint communityId, uint? communityOwnerId)
    {
        // client needs to be logged in to comment
        if (data.UserId is not uint clientId)
        {
            throw new ForbiddenHttpResponseException();
        }

        // since this new comment is a reply, we need to make sure the parent comment is not locked/deleted
        if (data.ParentCommentId is Guid newParentId)
        {
            var newParentValid = await ValidateParentId(newParentId);

            if (!newParentValid.Successful)
            {
                return newParentValid;
            }
        }

        // community moderators are allowed to comment
        if (communityOwnerId == clientId)
        {
            return new();
        }

        // if client is not a moderator, they must be a community member to create new comments
        var isMember = await IsMemberAsync(clientId, communityId);

        if (!isMember)
        {
            throw new ForbiddenHttpResponseException();
        }

        return new();
    }

    private async Task<ServiceResponse> ValidateParentId(Guid parentId)
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

        return new();
    }


    private async Task<bool> IsMemberAsync(uint clientId, uint communityId)
    {
        // check if the client is a member of the community
        var getMembership = await _communityMemberService.IsMemberAsync(clientId, communityId);

        getMembership.ThrowIfError();

        return getMembership.Data;
    }

    private async Task<ViewPost> GetPostAsync(Guid postId)
    {
        var getPost = await _postService.GetPostAsync(postId);

        if (!getPost.Successful)
        {
            throw new ServiceResponseException(getPost);
        }

        if (getPost.Data is not ViewPost post)
        {
            throw new NotFoundHttpResponseException();
        }

        HttpRequestItems.StorePost(_context, post);

        return post;
    }




}
