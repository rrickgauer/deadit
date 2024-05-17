using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Contracts;
using Deadit.Lib.Domain.Enum;
using static Deadit.Lib.Domain.Model.Votes;

namespace Deadit.Lib.Domain.TableView;

public class ViewVoteComment : IVoteScore,
    ITableView<ViewVoteComment, VoteComment>
{

    [SqlColumn("vote_comment_comment_id")]
    [CopyToProperty<VoteComment>(nameof(VoteComment.ItemId))]
    public Guid? VoteCommentId { get; set; }

    [SqlColumn("vote_comment_user_id")]
    [CopyToProperty<VoteComment>(nameof(VoteComment.UserId))]
    public uint? VoteCommentUserId { get; set; }

    [SqlColumn("vote_comment_vote_type")]
    [CopyToProperty<VoteComment>(nameof(VoteComment.VoteType))]
    public VoteType VoteCommentType { get; set; } = VoteType.Novote;

    [SqlColumn("vote_comment_created_on")]
    [CopyToProperty<VoteComment>(nameof(VoteComment.CreatedOn))]
    public DateTime? VoteCommentCreatedOn { get; set; }

    [SqlColumn("post_id")]
    public Guid? VoteCommentPostId { get; set; }

    [SqlColumn("vote_comment_username")]
    public string? VoteCommentUserName { get; set; }

    [SqlColumn("comment_count_votes_up")]
    public long VotesCountUp { get; set; } = 0;

    [SqlColumn("comment_count_votes_down")]
    public long VotesCountDown { get; set; } = 0;

    [SqlColumn("comment_count_votes_none")]
    public long VotesCountNone { get; set; } = 0;

    [SqlColumn("comment_count_votes_score")]
    public long VotesScore { get; set; } = 0;


    public static implicit operator VoteType(ViewVoteComment vote) => vote.VoteCommentType;

    #region - ITableView -

    public static explicit operator VoteComment(ViewVoteComment other) => other.CastToModel();

    #endregion

}


public static class ViewVoteCommentExtensions
{
    public static Dictionary<Guid, VoteType> ToVoteTypesDict(this IEnumerable<ViewVoteComment> votes)
    {
        var dict = votes.Where(v => v.VoteCommentId.HasValue).ToDictionary(v => v.VoteCommentId!.Value, v => v.VoteCommentType);

        return dict;
    }
}
