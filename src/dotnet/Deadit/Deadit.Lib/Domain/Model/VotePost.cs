namespace Deadit.Lib.Domain.Model;

using Deadit.Lib.Domain.Contracts;
using Deadit.Lib.Domain.Enum;
using System;

public class VotePost : IUserVoteable
{
    public Guid? PostId { get; set; }
    
    public uint? UserId { get; set; }
    public VoteType VoteType { get; set; }

    public DateTime CreatedOn { get; set; } = DateTime.Now;
}

