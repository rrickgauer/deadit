using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.TableView;
using Deadit.Lib.Utility;
using System.Data;

namespace Deadit.Lib.Mapping.Tables;

public class ViewVotePostMapper : TableMapper<ViewVotePost>
{
    public override ViewVotePost ToModel(DataRow row)
    {
        ViewVotePost result = new();

        result.VotePostId = row.Field<Guid?>(GetColumnName(nameof(result.VotePostId)));
        result.VotePostUserId = row.Field<uint?>(GetColumnName(nameof(result.VotePostUserId)));
        result.VotePostVoteType = (VoteType?)row.Field<ushort?>(GetColumnName(nameof(result.VotePostVoteType)));
        result.VotePostCreatedOn = row.Field<DateTime?>(GetColumnName(nameof(result.VotePostCreatedOn)));
        result.VotePostUserName = row.Field<string?>(GetColumnName(nameof(result.VotePostUserName)));
        result.VotePostCommunityId = row.Field<uint?>(GetColumnName(nameof(result.VotePostCommunityId)));
        result.VotePostCommunityName = row.Field<string?>(GetColumnName(nameof(result.VotePostCommunityName)));
        result.VotePostPostType = (PostType?)row.Field<ushort?>(GetColumnName(nameof(result.VotePostPostType)));

        row.SetVotingValues(result);

        return result;
    }
}
