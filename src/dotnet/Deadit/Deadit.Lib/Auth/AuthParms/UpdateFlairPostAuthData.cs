using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Forms;

namespace Deadit.Lib.Auth.AuthParms;

public class UpdateFlairPostAuthData
{
    public required uint ClientId { get; set; }
    public required FlairPostForm? FlairForm { get; set; }
    public required uint? FlairId { get; set; }
    public required FlairPostAuthValidationLevel ValidationLevel { get; set; }
}
