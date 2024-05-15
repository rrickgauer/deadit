using Deadit.Lib.Domain.Enum;

namespace Deadit.Lib.Domain.Parms;

public class GetCommentsParms
{
    public required Guid PostId { get; set; }
    public SortOption Sort {  get; set; } 
}
