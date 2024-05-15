using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Errors;
using Deadit.Lib.Domain.Forms;
using Deadit.Lib.Domain.Response;
using Deadit.Lib.Domain.TableView;
using Deadit.Lib.Service.Contracts;
using static Deadit.Lib.Auth.PermissionContracts;
using static Deadit.Lib.Domain.Model.Votes;

namespace Deadit.Lib.Auth;

public class VoteAuthData
{
    public required CreateVoteForm VoteForm { get; set; }
    public required uint ClientId { get; set; }
}

[AutoInject(AutoInjectionType.Scoped, InjectionProject.Always)]
public class VoteAuth(IPostService postService, ICommentService commentService) : IAsyncPermissionsAuth<VoteAuthData>
{
    private readonly IPostService _postService = postService;
    private readonly ICommentService _commentService = commentService;


    public async Task<ServiceResponse> HasPermissionAsync(VoteAuthData data)
    {
        try
        {
            var getVoteModel = data.VoteForm.ToVoteModel(data.ClientId);

            if (!getVoteModel.Successful)
            {
                return new(getVoteModel);
            }

            if (getVoteModel.Data is VotePost votePost)
            {
                return await CanVotePostAsync(votePost);
            }
            else if (getVoteModel.Data is VoteComment voteComment)
            {
                return await CanVoteComment(voteComment);
            }
            else
            {
                throw new NotImplementedException();
            }
   
        }
        catch(ServiceResponseException ex)
        {
            return new(ex);
        }
    }


    private async Task<ServiceResponse> CanVotePostAsync(VotePost vote)
    {

        if (vote.ItemId is not Guid postId)
        {
            throw new NotFoundHttpResponseException();
        }

        if (vote.UserId is not uint userId)
        {
            throw new NotFoundHttpResponseException();
        }

        // get the post
        var getPost = await _postService.GetPostAsync(postId);

        if (!getPost.Successful)
        {
            return new(getPost);
        }

        // ensure the post exists
        if (getPost.Data is not ViewPost post)
        {
            throw new NotFoundHttpResponseException();
        }

        return new();
    }

    private async Task<ServiceResponse> CanVoteComment(VoteComment vote)
    {
        throw new NotImplementedException();
    }
}
