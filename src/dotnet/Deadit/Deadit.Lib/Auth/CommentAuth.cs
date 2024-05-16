﻿using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Errors;
using Deadit.Lib.Domain.Response;
using Deadit.Lib.Domain.TableView;
using Deadit.Lib.Service.Contracts;
using static Deadit.Lib.Auth.AuthParms;
using static Deadit.Lib.Auth.PermissionContracts;

namespace Deadit.Lib.Auth;


[AutoInject(AutoInjectionType.Scoped, InjectionProject.Always)]
public class CommentAuth(ICommentService commentService) : IAsyncPermissionsAuth<CommentAuthData>
{
    private readonly ICommentService _commentService = commentService;

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
                AuthPermissionType.Delete => ValidateDelete(data, getComment),
                AuthPermissionType.Upsert => ValidateUpsert(data, getComment),
                AuthPermissionType.Get    => ValidateGet(data, getComment),
                _                         => throw new NotImplementedException(),
            };
        }
        catch(ServiceResponseException ex)
        {
            return new(ex);
        }
    }



    private static ServiceResponse ValidateDelete(CommentAuthData data, ServiceDataResponse<ViewComment> getCommet)
    {
        var comment = NotFoundHttpResponseException.ThrowIfNot<ViewComment>(getCommet.Data);

        if (comment.CommentAuthorId != data.UserId)
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


    private static ServiceResponse ValidateUpsert(CommentAuthData data, ServiceDataResponse<ViewComment> getCommet)
    {
        if (getCommet.Data is not ViewComment comment)
        {
            return new();
        }

        if (comment.CommentAuthorId != data.UserId)
        {
            throw new ForbiddenHttpResponseException();
        }


        return new();
    }



    private static ServiceResponse ValidateGet(CommentAuthData data, ServiceDataResponse<ViewComment> getComment)
    {
        if (getComment.Data is not ViewComment comment)
        {
            throw new NotFoundHttpResponseException();
        }

        if (comment.CommentId is not Guid)
        {
            throw new NotFoundHttpResponseException();
        }

        return new();
    }
}