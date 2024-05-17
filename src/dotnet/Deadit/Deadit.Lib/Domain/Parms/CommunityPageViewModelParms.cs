using Deadit.Lib.Domain.Contracts;
using Deadit.Lib.Domain.Other;

namespace Deadit.Lib.Domain.Parms;

public class CommunityPageViewModelParms : IClientIdArg, ICommunityNameArg
{
    public required string CommunityName { get; set; }
    public required uint? ClientId { get; set; }
    public required PostSorting PostSorting { get; set; }
}
