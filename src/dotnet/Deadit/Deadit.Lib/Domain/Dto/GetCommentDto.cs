﻿using Deadit.Lib.Domain.Contracts;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Other;
using Deadit.Lib.Domain.TableView;
using System.Text.Json.Serialization;

namespace Deadit.Lib.Domain.Dto;

public class GetCommentDto : ICreatedOnDifference, IVoteScore
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
    public long VotesCountUp { get; set; } = 0;
    public long VotesCountDown { get; set; } = 0;
    public long VotesCountNone { get; set; } = 0;
    public long VotesScore { get; set; } = 0;
    public bool CommentIsAuthor { get; set; } = false;
    public string CreatedOnDifferenceDisplay => DifferenceDisplayCalculator.FromNow(CommentCreatedOn ?? DateTime.UtcNow);
    public DateTime? CommentDeletedOn { get; set; }
    public VoteType UserVoteSelection { get; set; } = VoteType.Novote;
    public bool UserIsCommunityModerator { get; set; } = false;

    [JsonIgnore]
    public DateTime? CommentLockedOn { get; set; }
    public bool CommentIsLocked => CommentLockedOn.HasValue;
    
    [JsonIgnore]
    public DateTime? CommentRemovedOn { get; set; }
    public bool CommentIsRemoved => CommentRemovedOn.HasValue;


    public List<GetCommentDto> CommentReplies { get; set; } = new();

    #region - Methods -

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

    public void SetVoteTypeRecursive(Dictionary<Guid, VoteType> voteTypes)
    {
        CommentReplies.ForEach(r => r.SetVoteTypeRecursive(voteTypes));

        if (CommentId is not Guid commentId)
        {
            return;
        }

        if (voteTypes.TryGetValue(commentId, out VoteType voteType))
        {
            UserVoteSelection = voteType;
        }
    }

    public void SetUserIsCommunityModeratorRecursive(uint? clientId, uint communityModeratorId)
    {
        CommentReplies.ForEach(c => c.SetUserIsCommunityModeratorRecursive(clientId, communityModeratorId));

        if (!clientId.HasValue)
        {
            UserIsCommunityModerator = false;
            return;
        }

        UserIsCommunityModerator = CommentAuthorId == communityModeratorId;
    }

    #endregion
}


public static class GetCommentDtoExtensions
{
    public static List<GetCommentDto> BuildGetCommentDtos(this List<ViewComment> comments, uint? clientId, IEnumerable<ViewVoteComment> userVotes, uint communityModeratorId)
    {
        // need to put this into a function
        comments.ForEach(c => c.MaskDeletedInfo());

        var votes = userVotes.ToVoteTypesDict();

        var dtos = comments.Select(c =>
        {
            var dto = (GetCommentDto)c;
            dto.SetIsAuthorRecursive(clientId);
            dto.SetVoteTypeRecursive(votes);
            dto.SetUserIsCommunityModeratorRecursive(clientId, communityModeratorId);

            return dto;
        });

        return dtos.ToList();
    }
}



