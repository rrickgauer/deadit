namespace Deadit.Lib.Domain.Constants;


public interface IVoteButtonIcons
{
    public static abstract string Upvote { get; }
    public static abstract string Downvote { get; }
}


public class VoteButtonIcons : IVoteButtonIcons
{
    public static string Upvote => @"<i class=""bx bx-upvote""></i>";
    public static string Downvote => @"<i class=""bx bx-downvote""></i>";
}

public class VoteButtonIconsSolid : IVoteButtonIcons
{
    public static string Upvote => @"<i class=""bx bxs-upvote"" data-js-selected></i>";
    public static string Downvote => @"<i class=""bx bxs-downvote"" data-js-selected></i>";
}
