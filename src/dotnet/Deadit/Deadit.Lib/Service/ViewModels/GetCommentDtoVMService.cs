using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Dto;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Errors;
using Deadit.Lib.Domain.Parms;
using Deadit.Lib.Domain.Response;
using Deadit.Lib.Domain.TableView;
using Deadit.Lib.Service.Contracts;

namespace Deadit.Lib.Service.ViewModels;

[AutoInject(AutoInjectionType.Scoped, InjectionProject.WebGui)]
public class GetCommentDtoVMService(ICommentService commentService, ICommunityService communityService, ICommentVotesService commentVotesService) : IVMService<GetCommentDtoVMServiceParms, GetCommentDto>
{
    private readonly ICommentService _commentService = commentService;
    private readonly ICommunityService _communityService = communityService;
    private readonly ICommentVotesService _commentVotesService = commentVotesService;

    public async Task<ServiceDataResponse<GetCommentDto>> GetViewModelAsync(GetCommentDtoVMServiceParms args)
    {
        try
        {
            var comment = await GetCommentAsync(args);
            comment.MaskDeletedInfo();

            var communityId = NotFoundHttpResponseException.ThrowIfNot<uint>(comment.CommunityId);

            var community = await GetCommunityAsync(communityId);

            var commentDto = (GetCommentDto)comment;

            commentDto.SetIsAuthorRecursive(args.ClientId);
            commentDto.UserVoteSelection = await GetCommentVoteAsync(args);
            commentDto.SetUserIsCommunityModeratorRecursive(args.ClientId, communityId);
            commentDto.UserIsCommunityModerator = community.CommunityOwnerId == args.ClientId;

            return commentDto;
        }
        catch (ServiceResponseException ex)
        {
            return ex;
        }
    }


    private async Task<ViewComment> GetCommentAsync(GetCommentDtoVMServiceParms args)
    {
        var getComment = await _commentService.GetCommentAsync(args.CommentId);

        getComment.ThrowIfError();
        var comment = NotFoundHttpResponseException.ThrowIfNot<ViewComment>(getComment.Data);

        return comment;
    }

    private async Task<ViewCommunity> GetCommunityAsync(uint communityId)
    {
        var getCommunity = await _communityService.GetCommunityAsync(communityId);

        getCommunity.ThrowIfError();

        if (getCommunity.Data is not ViewCommunity community)
        {
            throw new NotFoundHttpResponseException();
        }

        return community;

    }

    private async Task<VoteType> GetCommentVoteAsync(GetCommentDtoVMServiceParms args)
    {

        if (args.ClientId is not uint clientId)
        {
            return VoteType.Novote;
        }

        var getVote = await _commentVotesService.GetVoteAsync(args.CommentId, clientId);

        getVote.ThrowIfError();

        if (getVote.Data is not ViewVoteComment voteComment)
        {
            return VoteType.Novote;
        }

        return voteComment.VoteCommentType;
    }



}
