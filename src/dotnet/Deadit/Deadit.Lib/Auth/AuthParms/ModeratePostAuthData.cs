namespace Deadit.Lib.Auth.AuthParms;

public class ModeratePostAuthData
{
    public required Guid PostId { get; set; }
    public required uint ClientId { get; set; }
}
