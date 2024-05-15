using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Contracts;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Model;
using Deadit.Lib.Domain.Other;
using System.Text.Json.Serialization;

namespace Deadit.Lib.Domain.TableView;


public class ViewPost : ViewCommunity, ICreatedUri, ICreatedOnDifference, IVoteScore,
    ITableView<ViewPost, PostText>,
    ITableView<ViewPost, PostLink>
{


    /*************************************************
     * DELETE THIS SHIT!!!!!
     * DELETE THIS SHIT!!!!!
     */

    [SqlColumn("post_count_comments")]
    public uint PostCountComments { get; set; } = 0;
    
    private static readonly Random Rand = new();

     /*************************************************
     * DELETE THIS SHIT!!!!!
     * DELETE THIS SHIT!!!!!
     */




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



    public int PostVotesScore { get; set; } = Rand.Next(-100, 100);



    [JsonIgnore]
    public virtual string PostBodyContent => string.Empty;


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
