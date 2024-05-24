namespace Deadit.Lib.Domain.Model;

using System;

public class Comment
{
    public Guid? Id { get; set; }
    public uint? AuthorId { get; set; }
    public Guid? PostId { get; set; }
    public string? Content { get; set; }
    public Guid? ParentId { get; set; }
    public DateTime? CreatedOn { get; set; }
    public DateTime? DeletedOn { get; set; }    
    public DateTime? LockedOn { get; set; }
    public DateTime? RemovedOn { get; set; }
}

