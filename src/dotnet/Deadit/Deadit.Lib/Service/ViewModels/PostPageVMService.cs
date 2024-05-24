using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Errors;
using Deadit.Lib.Domain.Parms;
using Deadit.Lib.Domain.Response;
using Deadit.Lib.Domain.TableView;
using Deadit.Lib.Domain.ViewModel;
using Deadit.Lib.Service.Contracts;

namespace Deadit.Lib.Service.ViewModels;

[AutoInject(AutoInjectionType.Scoped, InjectionProject.WebGui)]
public class PostPageVMService(IPostService postService, ICommentService commentService, IPostVotesService postVotesService) : IVMService<PostPageVMServiceParms, PostPageViewModel>
{
    private readonly IPostService _postService = postService;
    private readonly ICommentService _commentService = commentService;
    private readonly IPostVotesService _postVotesService = postVotesService;

    public async Task<ServiceDataResponse<PostPageViewModel>> GetViewModelAsync(PostPageVMServiceParms args)
    {

        // get the post
        ViewPost post;

        try
        {
            post = args.PostType switch
            {
                PostType.Text => await GetPostTextAsync(args.PostId),
                PostType.Link => await GetPostLinkAsync(args.PostId),
                _             => throw new NotImplementedException(),
            };
        }
        catch (ServiceResponseException ex)
        {
            return new(ex.Response);
        }

        post.HandlePostDeleted();

        // get comments
        var getComments = await _commentService.GetCommentsNestedAsync(new GetCommentsParms
        {
            PostId = args.PostId,
            SortOption = args.SortOption,
        });

        if (!getComments.Successful)
        {
            return new(getComments);
        }

        // get user's vote selection for the post

        VoteType userPostVote = VoteType.Novote;

        if (args.ClientId.HasValue)
        {
            var getPostVote = await _postVotesService.GetVoteAsync(args.PostId, args.ClientId.Value);

            if (!getPostVote.Successful)
            {
                return new(getPostVote);
            }

            userPostVote = getPostVote.Data?.VotePostVoteType ?? VoteType.Novote;
        }



        PostPageViewModel viewModel = new()
        {
            Post = post,
            IsAuthor = post.PostAuthorId == args.ClientId,
            Comments = getComments.Data ?? new(),
            ClientId = args.ClientId,
            IsLoggedIn = args.ClientId != null,
            SortOption = args.SortOption,
            UserPostVote = userPostVote,

        };

        return new(viewModel);


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
}
