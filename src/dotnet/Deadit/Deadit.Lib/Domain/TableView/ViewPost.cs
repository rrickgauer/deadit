using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Contracts;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Model;
using Deadit.Lib.Domain.Other;
using System.Text.Json.Serialization;

namespace Deadit.Lib.Domain.TableView;


public class ViewPost : ViewCommunity, ICreatedUri, ICreatedOnDifference, IVoteScore,
    ITableView<ViewPost, PostText>,
    ITableView<ViewPost, PostLink>,
    ITableView<ViewPost, FlairPost>
{

    protected const string DeletedContentText = "[Post deleted by author]";
    protected const string RemovedContentText = "[Post removed by moderator]";

    protected const string RemovedTitleText = "[Removed]";


    [SqlColumn("post_id")]
    [CopyToProperty<PostText>(nameof(PostText.Id))]
    [CopyToProperty<PostLink>(nameof(PostLink.Id))]
    public Guid? PostId { get; set; }

    [SqlColumn("post_community_id")]
    [CopyToProperty<PostText>(nameof(PostText.CommunityId))]
    [CopyToProperty<PostLink>(nameof(PostLink.CommunityId))]
    public uint? PostCommunityId { get; set; }

    [SqlColumn("post_title")]
    [CopyToProperty<PostText>(nameof(PostText.Title))]
    [CopyToProperty<PostLink>(nameof(PostLink.Title))]
    public string? PostTitle { get; set; }

    [SqlColumn("post_post_type")]
    public virtual PostType? PostType { get; set; }

    [SqlColumn("post_author_id")]
    [CopyToProperty<PostText>(nameof(PostText.AuthorId))]
    [CopyToProperty<PostLink>(nameof(PostLink.AuthorId))]
    public uint? PostAuthorId { get; set; }

    [SqlColumn("post_created_on")]
    [CopyToProperty<PostText>(nameof(PostText.CreatedOn))]
    [CopyToProperty<PostLink>(nameof(PostLink.CreatedOn))]
    public DateTime? PostCreatedOn { get; set; }

    [SqlColumn("post_deleted_on")]
    [CopyToProperty<PostText>(nameof(PostText.DeletedOn))]
    [CopyToProperty<PostLink>(nameof(PostLink.DeletedOn))]
    [JsonIgnore]
    public DateTime? PostDeletedOn { get; set; }

    [SqlColumn("post_archived_on")]
    [CopyToProperty<PostText>(nameof(PostText.ArchivedOn))]
    [CopyToProperty<PostLink>(nameof(PostLink.ArchivedOn))]
    [JsonIgnore]
    public DateTime? PostArchivedOn { get; set; }

    [SqlColumn("post_mod_removed_on")]
    [CopyToProperty<PostText>(nameof(PostText.RemovedOn))]
    [CopyToProperty<PostLink>(nameof(PostLink.RemovedOn))]
    [JsonIgnore]
    public DateTime? PostModRemovedOn { get; set; }


    [SqlColumn("post_locked_on")]
    [CopyToProperty<PostText>(nameof(PostText.LockedOn))]
    [CopyToProperty<PostLink>(nameof(PostLink.LockedOn))]
    [JsonIgnore]
    public DateTime? PostLockedOn { get; set; }


    [SqlColumn("post_count_comments")]
    public uint PostCountComments { get; set; } = 0;

    #region - Flair Post -

    [SqlColumn("post_flair_post_id")]
    [CopyToProperty<PostText>(nameof(PostText.FlairPostId))]
    [CopyToProperty<PostLink>(nameof(PostLink.FlairPostId))]
    [CopyToProperty<FlairPost>(nameof(FlairPost.Id))]
    public uint? FlairPostId { get; set; }

    [SqlColumn("flair_post_community_id")]
    [CopyToProperty<FlairPost>(nameof(FlairPost.CommunityId))]
    public uint? FlairPostCommunityId { get; set; }

    [SqlColumn("flair_post_name")]
    [CopyToProperty<FlairPost>(nameof(FlairPost.Name))]
    public string? FlairPostName { get; set; }

    [SqlColumn("flair_post_color")]
    [CopyToProperty<FlairPost>(nameof(FlairPost.Color))]
    public string? FlairPostColor { get; set; }

    [SqlColumn("flair_post_created_on")]
    [CopyToProperty<FlairPost>(nameof(FlairPost.CreatedOn))]
    public DateTime? FlairPostCreatedOn { get; set; }

    #endregion


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

    //[JsonIgnore]
    public virtual string? PostBodyContent => null;


    public bool PostIsArchived => PostArchivedOn.HasValue;
    public bool PostIsRemoved => PostModRemovedOn.HasValue;
    public bool PostIsDeleted => PostDeletedOn.HasValue;
    public bool PostIsLocked => PostLockedOn.HasValue;


    public virtual void HandlePostDeleted()
    {
        if (PostIsRemoved)
        {
            PostTitle = RemovedTitleText;
        }
    }


    #region - ICreatedUri -

    [JsonPropertyName("postUriWeb")]
    public virtual string UriWeb => $"/c/{CommunityName}/posts/{PostId}";

    [JsonPropertyName("postUriApi")]
    public virtual string UriApi => $"/communities/{CommunityName}/posts/{PostId}";

    public string GetCreatedUri(string uriPrefix)
    {
        return $"{uriPrefix}/communities/{CommunityName}/{PostId}";
    }

    #endregion

    #region - ICreatedOnDifference -

    public string CreatedOnDifferenceDisplay => DifferenceDisplayCalculator.FromNow(PostCreatedOn ?? DateTime.UtcNow);

    #endregion


    #region - ITableView -

    public static explicit operator PostText(ViewPost other) => other.CastToModel<ViewPost, PostText>();
    public static explicit operator PostLink(ViewPost other) => other.CastToModel<ViewPost, PostLink>();
    public static explicit operator FlairPost(ViewPost other) => other.CastToModel<ViewPost, FlairPost>();

    public static explicit operator Post(ViewPost other)
    {
        ArgumentNullException.ThrowIfNull(other.PostType, nameof(other.PostType));

        if (other.PostType == Enum.PostType.Link)
        {
            return (PostLink)other;
        }

        return (PostText)other;
    }

    #endregion
}
