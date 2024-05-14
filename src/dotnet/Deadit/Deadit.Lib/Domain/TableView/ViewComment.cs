using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Contracts;
using Deadit.Lib.Domain.Dto;
using Deadit.Lib.Domain.Model;
using Deadit.Lib.Domain.Other;
using Deadit.Lib.Utility;
using System.Text.Json.Serialization;

namespace Deadit.Lib.Domain.TableView;

public class ViewComment : ICreatedOnDifference, IVoteScore,
    ITableView<ViewComment, GetCommentDto>,
    ITableView<ViewComment, Comment>
{
    [SqlColumn("comment_id")]
    [CopyToProperty<Comment>(nameof(Comment.Id))]
    [CopyToProperty<GetCommentDto>(nameof(GetCommentDto.CommentId))]
    public Guid? CommentId { get; set; }

    [SqlColumn("comment_author_id")]
    [CopyToProperty<Comment>(nameof(Comment.AuthorId))]
    [CopyToProperty<GetCommentDto>(nameof(GetCommentDto.CommentAuthorId))]
    public uint? CommentAuthorId { get; set; }

    [SqlColumn("comment_post_id")]
    [CopyToProperty<Comment>(nameof(Comment.PostId))]
    [CopyToProperty<GetCommentDto>(nameof(GetCommentDto.CommentPostId))]
    public Guid? CommentPostId { get; set; }

    [SqlColumn("comment_content")]
    [CopyToProperty<Comment>(nameof(Comment.Content))]
    [CopyToProperty<GetCommentDto>(nameof(GetCommentDto.CommentContent))]
    public string? CommentContent { get; set; }

    [SqlColumn("comment_parent_id")]
    [CopyToProperty<Comment>(nameof(Comment.ParentId))]
    [CopyToProperty<GetCommentDto>(nameof(GetCommentDto.CommentParentId))]
    public Guid? CommentParentId { get; set; }

    [SqlColumn("comment_created_on")]
    [CopyToProperty<Comment>(nameof(Comment.CreatedOn))]
    [CopyToProperty<GetCommentDto>(nameof(GetCommentDto.CommentCreatedOn))]
    public DateTime? CommentCreatedOn { get; set; }


    [SqlColumn("comment_deleted_on")]
    [CopyToProperty<Comment>(nameof(Comment.DeletedOn))]
    [CopyToProperty<GetCommentDto>(nameof(GetCommentDto.CommentDeletedOn))]
    public DateTime? CommentDeletedOn { get; set; }


    [CopyToProperty<GetCommentDto>(nameof(GetCommentDto.CommentAuthorUsername))]
    [SqlColumn("comment_author_username")]
    public string? CommentAuthorUsername { get; set; }

    [CopyToProperty<GetCommentDto>(nameof(GetCommentDto.CommunityId))]
    [SqlColumn("community_id")]
    public uint? CommunityId { get; set; }

    [CopyToProperty<GetCommentDto>(nameof(GetCommentDto.CommunityName))]
    [SqlColumn("community_name")]
    public string? CommunityName { get; set; }

    #region - IVoteScore -

    [SqlColumn("comment_count_votes_up")]
    [CopyToProperty<GetCommentDto>(nameof(GetCommentDto.VotesCountUp))]
    public long VotesCountUp { get; set; } = 0;

    [SqlColumn("comment_count_votes_down")]
    [CopyToProperty<GetCommentDto>(nameof(GetCommentDto.VotesCountDown))]
    public long VotesCountDown { get; set; } = 0;

    [SqlColumn("comment_count_votes_none")]
    [CopyToProperty<GetCommentDto>(nameof(GetCommentDto.VotesCountNone))]
    public long VotesCountNone { get; set; } = 0;

    [SqlColumn("comment_count_votes_score")]
    [CopyToProperty<GetCommentDto>(nameof(GetCommentDto.VotesScore))]
    public long VotesScore { get; set; } = 0;

    #endregion

    public List<ViewComment> CommentReplies { get; set; } = new();

    [JsonIgnore]
    public bool HasReplies => CommentReplies.Count > 0;

    [JsonIgnore]
    public bool IsTopLevel => CommentParentId == null;

    [JsonIgnore]
    public bool IsDeleted => CommentDeletedOn.HasValue;


    public void GenerateContentHtml()
    {
        if (!string.IsNullOrWhiteSpace(CommentContent))
        {
            CommentContent = MarkdownUtility.ToHtml(CommentContent);
        }
    }


    public void MaskDeletedInfo()
    {
        foreach (var reply in CommentReplies)
        {
            reply.MaskDeletedInfo();
        }


        if (IsDeleted)
        {
            string deletedText = "[deleted]";
            CommentAuthorUsername = deletedText;
            CommentContent = deletedText;
            CommentAuthorId = null;
        }
    }



    #region - ICreatedOnDifference -

    [JsonIgnore]
    public string CreatedOnDifferenceDisplay => DifferenceDisplayCalculator.FromNow(CommentCreatedOn ?? DateTime.UtcNow);

    #endregion

    #region - ITableView -

    public static explicit operator Comment(ViewComment other) => other.CastToModel<ViewComment, Comment>();

    public static explicit operator GetCommentDto(ViewComment other)
    {
        var dto = other.CastToModelTry<ViewComment, GetCommentDto>();

        dto.CommentReplies = other.CommentReplies.Select(r => (GetCommentDto)r).ToList();

        return dto;
    }

    #endregion



}
