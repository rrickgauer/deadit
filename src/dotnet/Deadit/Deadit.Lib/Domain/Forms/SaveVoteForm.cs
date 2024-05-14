using Deadit.Lib.Domain.Enum;

namespace Deadit.Lib.Domain.Forms;

public class SaveVoteForm
{
    public required Guid ItemId { get; set; }
    public required uint UserId {  get; set; }
    public required VoteType VoteType { get; set; }
}
