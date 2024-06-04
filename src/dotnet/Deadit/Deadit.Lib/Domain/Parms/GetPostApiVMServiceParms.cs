using Deadit.Lib.Domain.Enum;

namespace Deadit.Lib.Domain.Parms;

public class GetPostApiVMServiceParms
{
    public required Guid PostId { get; set; }
    public required uint? ClientId { get; set; }
    public required PostType PostType { get; set; }
    public required SortOption SortOption { get; set; }
}
