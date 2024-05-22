using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Dto;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Errors;
using Deadit.Lib.Domain.Parms;
using Deadit.Lib.Domain.Response;
using Deadit.Lib.Domain.TableView;
using Deadit.Lib.Domain.ViewModel;
using Deadit.Lib.Service.Contracts;
using System.Runtime.InteropServices;

namespace Deadit.Lib.Service.ViewModels;


[AutoInject(AutoInjectionType.Scoped, InjectionProject.WebGui)]
public class GetCommentsApiVMService(ICommentService commentService, ICommentVotesService commentVotesService, IPostService postService) : IVMService<GetCommentsApiVMServiceParms, GetCommentsApiViewModel>
{
    private readonly ICommentService _commentService = commentService;
    private readonly ICommentVotesService _commentVotesService = commentVotesService;
    private readonly IPostService _postService = postService;

    public async Task<ServiceDataResponse<GetCommentsApiViewModel>> GetViewModelAsync(GetCommentsApiVMServiceParms args)
    {
        try
        {
            var getComments = await _commentService.GetCommentsNestedAsync(new()
            {
                PostId = args.PostId,
                SortOption = args.SortOption,
            });


            if (!getComments.Successful)
            {
                return new(getComments);
            }

            var comments = getComments.Data ?? new();
            var votes = await GetUserCommentVotesAsync(args.PostId, args.ClientId);
            var dtos = comments.BuildGetCommentDtos(args.ClientId, votes);


            var post = await GetPostAsync(args);

            GetCommentsApiViewModel viewModel = new()
            {
                Comments = dtos.ToList(),
                IsLoggedIn = args.ClientId.HasValue,
                PostIsDeleted = post.PostDeletedOn.HasValue,
            };

            return viewModel;
        }
        catch (ServiceResponseException ex)
        {
            return ex;
        }
    }

    private async Task<List<ViewVoteComment>> GetUserCommentVotesAsync(Guid postId, uint? clientId)
    {

        if (!clientId.HasValue)
        {
            return new();
        }

        var getVotes = await _commentVotesService.GetUserCommentVotesInPost(postId, clientId.Value);

        if (!getVotes.Successful)
        {
            throw new ServiceResponseException(getVotes);
        }

        return getVotes.Data ?? new();
    }

    
    
    private async Task<ViewPost> GetPostAsync(GetCommentsApiVMServiceParms args)
    {
        var getPost = await _postService.GetPostAsync(args.PostId);
        
        if (!getPost.Successful)
        {
            throw new ServiceResponseException(getPost);
        }

        var post = NotFoundHttpResponseException.ThrowIfNot<ViewPost>(getPost.Data);

        return post;
    }



}
