namespace Deadit.Lib.Domain.Contracts;

public interface IVoteScore
{
    public long VotesCountUp { get; set; }
    public long VotesCountDown { get; set; }
    public long VotesCountNone { get; set; }
    public long VotesScore { get; set; }


    public static readonly List<string> PropertyNames = typeof(IVoteScore).GetProperties().Select(p => p.Name).ToList();
}


