using Deadit.Lib.Domain.Enum;

namespace Deadit.Lib.Domain.Model;

public class Votes
{
    public class VoteBase
    {
        public virtual Guid? ItemId { get; set; }
        public virtual uint? UserId { get; set; }
        public virtual VoteType? VoteType { get; set; }
        public virtual DateTime? CreatedOn { get; set; }
    }

    public abstract class Vote : VoteBase
    {
        public abstract VoteItem VoteItem { get; }
    }

    public class VoteComment : Vote
    {
        public override VoteItem VoteItem => VoteItem.Comment;
    }


    public class VotePost : Vote
    {
        public override VoteItem VoteItem => VoteItem.Post;
    }
}


