using Deadit.Lib.Domain.Contracts;
using Deadit.Lib.Domain.Enum;

namespace Deadit.Lib.Domain.Parms;

public class GetCommentsApiVMServiceParms : IPostIdArg, IClientIdArg, ISortOptionArg
{
    public required uint? ClientId { get; set; }
    public required Guid PostId { get; set; }
    public required SortOption SortOption { get; set; }

}
