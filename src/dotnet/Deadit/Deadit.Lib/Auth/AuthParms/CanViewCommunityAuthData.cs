namespace Deadit.Lib.Auth.AuthParms;

public class CanViewCommunityAuthData
{
    public required string CommunityName { get; set; }
    public required uint? ClientId { get; set; }
}
