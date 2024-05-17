using Deadit.Lib.Domain.Contracts;

namespace Deadit.Lib.Domain.Parms;

public class GetCommentDtoVMServiceParms : IClientIdArg
{
    public required Guid CommentId { get; set; }
    public required uint? ClientId { get; set; }
}
