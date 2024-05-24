namespace Deadit.Lib.Domain.Enum;

[Flags]
public enum PostStatus
{
    None              = 0,
    DeletedByAuthor   = 1 << 0,
    ModRemoved        = 1 << 1,
    ModCommentsLocked = 1 << 2,
}
