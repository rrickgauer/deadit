namespace Deadit.Lib.Domain.Constants;


public interface IVoteButtonIcons
{
    public static abstract string Upvote { get; }
    public static abstract string Downvote { get; }
}


public class VoteButtonIcons : IVoteButtonIcons
{
    public static string Upvote => Icons.Upvote;
    public static string Downvote => Icons.Downvote;
}

public class VoteButtonIconsSolid : IVoteButtonIcons
{
    public static string Upvote => Icons.UpvoteSolid;
    public static string Downvote => Icons.DownvoteSolid;
}
