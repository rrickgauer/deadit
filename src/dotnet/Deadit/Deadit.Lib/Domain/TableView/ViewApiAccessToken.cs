using Deadit.Lib.Domain.Attributes;

namespace Deadit.Lib.Domain.TableView;


public class ViewApiAccessToken : ViewUser
{
    [SqlColumn("token_id")]
    public uint? TokenId { get; set; }

    [SqlColumn("token_token")]
    public Guid? Token { get; set; }

    [SqlColumn("token_user_id")]
    public uint? TokenUserId { get; set; }

    [SqlColumn("token_created_on")]
    public DateTime? TokenCreatedOn { get; set; }
}
