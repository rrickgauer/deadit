using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.TableView;
using Deadit.Lib.Utility;
using System.Data;

namespace Deadit.Lib.Mapping.Tables;

public class ViewVoteCommentMapper : TableMapper<ViewVoteComment>
{
    public override ViewVoteComment ToModel(DataRow row)
    {
        ViewVoteComment result = new();

        result.VoteCommentId = row.Field<Guid?>(GetColumnName(nameof(result.VoteCommentId)));
        result.VoteCommentUserId = row.Field<uint?>(GetColumnName(nameof(result.VoteCommentUserId)));
        result.VoteCommentType = (VoteType)row.Field<ushort>(GetColumnName(nameof(result.VoteCommentType)));
        result.VoteCommentCreatedOn = row.Field<DateTime?>(GetColumnName(nameof(result.VoteCommentCreatedOn)));
        result.VoteCommentUserName = row.Field<string?>(GetColumnName(nameof(result.VoteCommentUserName)));
        result.VoteCommentPostId = row.Field<Guid?>(GetColumnName(nameof(result.VoteCommentPostId)));

        row.SetVotingValues(result);

        return result;
    }
}
