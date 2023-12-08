using Deadit.Lib.Domain.Enum;

namespace Deadit.Lib.Domain.Contracts;

public interface IUserVoteable
{
    public uint? UserId { get; set; }
    public VoteType VoteType { get; set; }
}
