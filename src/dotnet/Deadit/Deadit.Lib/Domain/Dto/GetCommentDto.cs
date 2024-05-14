using Deadit.Lib.Domain.Contracts;
using Deadit.Lib.Domain.Other;
using System.Text.Json.Serialization;

namespace Deadit.Lib.Domain.Dto;

public class GetCommentDto : ICreatedOnDifference
{
    public Guid? CommentId { get; set; }
    public uint? CommentAuthorId { get; set; }
    public Guid? CommentPostId { get; set; }
    public string? CommentContent { get; set; }
    public Guid? CommentParentId { get; set; }
    public DateTime? CommentCreatedOn { get; set; }
    public string? CommentAuthorUsername { get; set; }
    public uint? CommunityId { get; set; }
    public string? CommunityName { get; set; }

    

    public bool CommentIsAuthor { get; set; } = false;
    
    public string CreatedOnDifferenceDisplay => DifferenceDisplayCalculator.FromNow(CommentCreatedOn ?? DateTime.UtcNow);

    public List<GetCommentDto> CommentReplies { get; set; } = new();

    public DateTime? CommentDeletedOn { get; set; }


    public void SetIsAuthorRecursive(uint? clientId)
    {
        foreach(var reply in CommentReplies)
        {
            reply.SetIsAuthorRecursive(clientId);
        }


        if (CommentDeletedOn.HasValue)
        {
            CommentIsAuthor = false;
        }
        else
        {
            CommentIsAuthor = CommentAuthorId == clientId;
        }
    }
}

