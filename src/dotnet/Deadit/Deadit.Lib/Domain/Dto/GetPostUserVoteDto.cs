using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.TableView;

namespace Deadit.Lib.Domain.Dto;

public class GetPostUserVoteDto
{
    public required ViewPost Post { get; init; }
    public required VoteType UserVoteType { get; init; }

    public static implicit operator ViewPost(GetPostUserVoteDto dto) => dto.Post;
}


public static class GetPostUserVoteDtoExtensions
{
    public static List<GetPostUserVoteDto> BuildGetPostUserVoteDtos(this IEnumerable<ViewPost> posts, List<ViewVotePost> userVotes)
    {
        var userVotesDict = userVotes.ToVotesDict();

        List<GetPostUserVoteDto> result = new();

        foreach (var post in posts)
        {
            if (post.PostId is not Guid postId)
            {
                continue;
            }

            VoteType userVote = userVotesDict.ContainsKey(postId) ? userVotesDict[postId] : VoteType.Novote;

            result.Add(new()
            {
                Post = post,
                UserVoteType = userVote,
            });
        }

        return result;
    }
}
