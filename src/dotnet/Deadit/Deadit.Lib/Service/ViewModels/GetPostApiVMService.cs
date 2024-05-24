using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Dto;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Errors;
using Deadit.Lib.Domain.Parms;
using Deadit.Lib.Domain.Response;
using Deadit.Lib.Domain.TableView;
using Deadit.Lib.Domain.ViewModel;
using Deadit.Lib.Service.Contracts;

namespace Deadit.Lib.Service.ViewModels;



[AutoInject(AutoInjectionType.Scoped, InjectionProject.WebGui)]
public class GetPostApiVMService(IPostService postService, ICommentService commentService, ICommentVotesService commentVotesService, IPostVotesService postVotesService) : IVMService<GetPostApiVMServiceParms, GetPostApiViewModel>
{
    private readonly IPostService _postService = postService;
    private readonly ICommentService _commentService = commentService;
    private readonly ICommentVotesService _commentVotesService = commentVotesService;
    private readonly IPostVotesService _postVotesService = postVotesService;

    public async Task<ServiceDataResponse<GetPostApiViewModel>> GetViewModelAsync(GetPostApiVMServiceParms args)
    {

        try
        {
            var post = await GetPostAsync(args);

            var postModeratorId = NotFoundHttpResponseException.ThrowIfNot<uint>(post.CommunityOwnerId);
            
            var comments = await GetCommentsAsync(args, postModeratorId);

            var postVote = await GetClientPostVoteSelectionAsync(args);

            GetPostApiViewModel viewModel = new()
            {
                IsLoggedIn = args.ClientId.HasValue,
                IsCommunityModerator = post.CommunityOwnerId == args.ClientId,
                IsPostAuthor = args.ClientId == post.PostAuthorId,
                Post = post,
                PostIsDeleted = post.PostDeletedOn.HasValue,
                IsPostModRemoved = post.PostModRemovedOn.HasValue,
                Comments = comments,
                ClientPostVote = postVote,
                PostIsLocked = post.PostIsLocked,
            };

            return new(viewModel);
        }
        catch (ServiceResponseException ex)
        {
            return new(ex.Response);
        }
    }




    private async Task<ViewPost> GetPostAsync(GetPostApiVMServiceParms args)
    {

        ViewPost post = args.PostType switch
        {
            PostType.Text => await GetPostTextAsync(args.PostId),
            PostType.Link => await GetPostLinkAsync(args.PostId),
            _ => throw new NotImplementedException(),
        };

        post.HandlePostDeleted();

        return post;

    }


    private async Task<ViewPostText> GetPostTextAsync(Guid postId)
    {
        // get the post
        var getPost = await _postService.GetTextPostAsync(postId);

        if (!getPost.Successful)
        {
            throw new ServiceResponseException(getPost);
        }

        if (getPost.Data is not ViewPostText post)
        {
            throw new NotFoundHttpResponseException();
        }

        return post;
    }

    private async Task<ViewPostLink> GetPostLinkAsync(Guid postId)
    {
        // get the post
        var getPost = await _postService.GetLinkPostAsync(postId);

        if (!getPost.Successful)
        {
            throw new ServiceResponseException(getPost);
        }

        if (getPost.Data is not ViewPostLink post)
        {
            throw new NotFoundHttpResponseException();
        }

        return post;
    }



    private async Task<List<GetCommentDto>> GetCommentsAsync(GetPostApiVMServiceParms args, uint communityModeratorId)
    {
        var getComments = await _commentService.GetCommentsNestedAsync(new()
        {
            PostId = args.PostId,
            SortOption = args.SortOption,
        });

        getComments.ThrowIfError();

        var comments = getComments.Data ?? new();
        var votes = await GetUserCommentVotesAsync(args);

        var dtos = comments.BuildGetCommentDtos(args.ClientId, votes, communityModeratorId);
        return dtos;
    }


    private async Task<List<ViewVoteComment>> GetUserCommentVotesAsync(GetPostApiVMServiceParms args)
    {

        if (args.ClientId is not uint clientId)
        {
            return new();
        }

        var getVotes = await _commentVotesService.GetUserCommentVotesInPost(args.PostId, clientId);

        if (!getVotes.Successful)
        {
            throw new ServiceResponseException(getVotes);
        }

        return getVotes.Data ?? new();
    }



    private async Task<VoteType> GetClientPostVoteSelectionAsync(GetPostApiVMServiceParms args)
    {
        if (args.ClientId is not uint clientId)
        {
            return VoteType.Novote;
        }


        var getPostVote = await _postVotesService.GetVoteAsync(args.PostId, clientId);

        getPostVote.ThrowIfError();

        return getPostVote.Data?.VotePostVoteType ?? VoteType.Novote;
    }

}
