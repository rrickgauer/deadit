using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Contracts;
using Deadit.Lib.Domain.Enum;
using static Deadit.Lib.Domain.Model.Votes;

namespace Deadit.Lib.Domain.TableView;

public class ViewVotePost :  IVoteScore,
    ITableView<ViewVotePost, VotePost>
{
    [SqlColumn("vote_post_post_id")]
    [CopyToProperty<VotePost>(nameof(VotePost.ItemId))]
    public Guid? VotePostId { get; set; }

    [SqlColumn("vote_post_user_id")]
    [CopyToProperty<VotePost>(nameof(VotePost.UserId))]
    public uint? VotePostUserId { get; set; }

    [SqlColumn("vote_post_vote_type")]
    [CopyToProperty<VotePost>(nameof(VotePost.VoteType))]
    public VoteType? VotePostVoteType { get; set; }

    [SqlColumn("vote_post_created_on")]
    [CopyToProperty<VotePost>(nameof(VotePost.CreatedOn))]
    public DateTime? VotePostCreatedOn { get; set; }

    [SqlColumn("vote_post_user_username")]
    public string? VotePostUserName { get; set; }

    [SqlColumn("post_community_id")]
    public uint? VotePostCommunityId { get; set; }

    [SqlColumn("post_community_name")]
    public string? VotePostCommunityName { get; set; }

    [SqlColumn("post_post_type")]
    public PostType? VotePostPostType { get; set; }

    #region - IVoteScore -

    [SqlColumn("post_count_votes_upvotes")]
    public long VotesCountUp { get; set; } = 0;

    [SqlColumn("post_count_votes_downvotes")]
    public long VotesCountDown { get; set; } = 0;

    [SqlColumn("post_count_votes_novotes")]
    public long VotesCountNone { get; set; } = 0;

    [SqlColumn("post_count_votes_score")]
    public long VotesScore { get; set; } = 0;

    #endregion



    #region - ITableView -

    public static explicit operator VotePost(ViewVotePost other) => other.CastToModel();

    #endregion

}


public static class ViewVotePostExtensions
{
    public static Dictionary<Guid, VoteType> ToVotesDict(this IEnumerable<ViewVotePost> votes)
    {        
        var validVotes = votes.Where(v => v.VotePostId.HasValue && v.VotePostVoteType != null);

        return validVotes.ToDictionary(v => v.VotePostId!.Value, v => v.VotePostVoteType!.Value);
    }
}
