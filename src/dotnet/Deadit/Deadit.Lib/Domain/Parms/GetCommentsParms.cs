using Deadit.Lib.Domain.Contracts;
using Deadit.Lib.Domain.Enum;

namespace Deadit.Lib.Domain.Parms;

public class GetCommentsParms : ISortOption, IPostId
{
    public required Guid PostId { get; set; }
    public required SortOption SortOption {  get; set; } 
}
