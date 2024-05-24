using Deadit.Lib.Domain.Contracts;

namespace Deadit.Lib.Domain.Parms;

public class GetJoinedCommunitiesPageParms : IClientId
{
    public required uint? ClientId { get; set; }
}
