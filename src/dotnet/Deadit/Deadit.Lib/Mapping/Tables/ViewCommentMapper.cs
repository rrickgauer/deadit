using Deadit.Lib.Domain.TableView;
using Deadit.Lib.Utility;
using System.Data;

namespace Deadit.Lib.Mapping.Tables;

public class ViewCommentMapper : TableMapper<ViewComment>
{
    public override ViewComment ToModel(DataRow row)
    {
        ViewComment result = new()
        {
            CommentId             = row.Field<Guid?>(GetColumnName(nameof(result.CommentId))),
            CommentAuthorId       = row.Field<uint?>(GetColumnName(nameof(result.CommentAuthorId))),
            CommentPostId         = row.Field<Guid?>(GetColumnName(nameof(result.CommentPostId))),
            CommentContent        = row.Field<string?>(GetColumnName(nameof(result.CommentContent))),
            CommentParentId       = row.Field<Guid?>(GetColumnName(nameof(result.CommentParentId))),
            CommentCreatedOn      = row.Field<DateTime?>(GetColumnName(nameof(result.CommentCreatedOn))),
            CommentAuthorUsername = row.Field<string?>(GetColumnName(nameof(result.CommentAuthorUsername))),
            CommunityId           = row.Field<uint?>(GetColumnName(nameof(result.CommunityId))),
            CommunityName         = row.Field<string?>(GetColumnName(nameof(result.CommunityName))),
            CommentDeletedOn      = row.Field<DateTime?>(GetColumnName(nameof(result.CommentDeletedOn))),
        };

        row.SetVotingValues(result);

        return result;
    }
}
