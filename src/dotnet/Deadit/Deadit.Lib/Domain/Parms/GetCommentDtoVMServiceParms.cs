using Deadit.Lib.Domain.Contracts;

namespace Deadit.Lib.Domain.Parms;

public class GetCommentDtoVMServiceParms : IClientId
{
    public required Guid CommentId { get; set; }
    public required uint? ClientId { get; set; }
    //public required string CommunityName { get; set; }
}
