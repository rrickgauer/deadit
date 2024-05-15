using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Response;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using static Deadit.Lib.Domain.Model.Votes;

namespace Deadit.Lib.Domain.Forms;

public class CreateVoteForm
{
    [BindRequired]
    public required VoteType VoteType { get; set; }

    public Guid? PostId { get; set; }
    public Guid? CommentId { get; set; }


    public ServiceDataResponse<Vote> ToVoteModel(uint userId)
    {
        if (PostId == null && CommentId == null)
        {
            return new(ErrorCode.VotingMissingRequiredParm);
        }

        if (PostId != null && CommentId != null)
        {
            return new(ErrorCode.VotingBothParmsGiven);
        }

        Vote vote = PostId.HasValue ? new VotePost() { ItemId = PostId.Value } : new VoteComment() { ItemId = CommentId!.Value };

        vote.CreatedOn = DateTime.UtcNow;
        vote.UserId = userId;
        vote.VoteType = VoteType;

        return new(vote);

    }
}