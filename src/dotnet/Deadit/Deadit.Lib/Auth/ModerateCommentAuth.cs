using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Errors;
using Deadit.Lib.Domain.Response;
using Deadit.Lib.Domain.TableView;
using Deadit.Lib.Service.Contracts;
using static Deadit.Lib.Auth.AuthParms;
using static Deadit.Lib.Auth.PermissionContracts;

namespace Deadit.Lib.Auth;



[AutoInject(AutoInjectionType.Scoped, InjectionProject.WebGui)]
public class ModerateCommentAuth(ICommentService commentService, ICommunityService communityService) : IAsyncPermissionsAuth<ModerateCommentAuthData>
{
    private readonly ICommentService _commentService = commentService;
    private readonly ICommunityService _communityService = communityService;

    public async Task<ServiceResponse> HasPermissionAsync(ModerateCommentAuthData data)
    {

        try
        {

            var comment = await GetCommentAsync(data);

            if (comment.CommunityName is not string communityName)
            {
                throw new NotFoundHttpResponseException();
            }

            var community = await GetCommunityAsync(communityName);

            if (communityName != data.CommunityName)
            {
                throw new NotFoundHttpResponseException();
            }

            if (comment.CommentPostId != data.PostId)
            {
                throw new NotFoundHttpResponseException();
            }

            if (community.CommunityOwnerId != data.ClientId)
            {
                throw new ForbiddenHttpResponseException();
            }

            return new();

        }
        catch (ServiceResponseException ex)
        {
            return ex;
        }


        throw new NotImplementedException();
    }


    private async Task<ViewComment> GetCommentAsync(ModerateCommentAuthData data)
    {
        var getComment = await _commentService.GetCommentAsync(data.CommentId);

        getComment.ThrowIfError();

        if (getComment.Data is not ViewComment comment)
        {
            throw new NotFoundHttpResponseException();
        }

        return comment;
    }


    private async Task<ViewCommunity> GetCommunityAsync(string communityName)
    {
        var getCommunity = await _communityService.GetCommunityAsync(communityName);

        getCommunity.ThrowIfError();

        if (getCommunity.Data is not ViewCommunity community)
        {
            throw new NotFoundHttpResponseException();
        }

        return community;
    }
}