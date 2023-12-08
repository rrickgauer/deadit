using Deadit.Lib.Domain.Contracts;
using Deadit.Lib.Domain.Enum;
using System;

namespace Deadit.Lib.Domain.Model;

public class VoteComment : IUserVoteable
{
    public Guid? CommentId { get; set; }

    public uint? UserId { get; set; }
    public VoteType VoteType { get; set; }

    public DateTime CreatedOn { get; set; } = DateTime.Now;
    
}

